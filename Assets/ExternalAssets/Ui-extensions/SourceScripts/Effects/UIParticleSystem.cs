/// Credit glennpow
/// Sourced from - http://forum.unity3d.com/threads/free-script-particle-systems-in-ui-screen-space-overlay.406862/
/// *Note - experimental.  Currently renders in scene view and not game view.

using System;
using System.Diagnostics;
using CustomLogger;
using ExternalAssets.SourceScripts.Utilities;

namespace UnityEngine.UI.Extensions
{
#if UNITY_5_3_OR_NEWER
    [ExecuteInEditMode]
    [RequireComponent(typeof(CanvasRenderer))]
    [RequireComponent(typeof(ParticleSystem))]
    public class UIParticleSystem : MaskableGraphic
    {
        public Texture particleTexture;
        public Sprite particleSprite;
        public float ScaleAmount = 1;

        private Transform _transform;
        private ParticleSystem _particleSystem;
        private ParticleSystem.Particle[] _particles;
        private Vector4 _uv = Vector4.zero;
        private ParticleSystem.TextureSheetAnimationModule _textureSheetAnimation;
        private int _textureSheetAnimationFrames;
        private Vector2 _textureSheedAnimationFrameSize;

        private ParticleSystemRenderer _renderer;

        private const float Tolerance = 0.000001f;

        public override Texture mainTexture
        {
            get
            {
                if (particleTexture)
                {
                    return particleTexture;
                }

                if (particleSprite)
                {
                    return particleSprite.texture;
                }

                return null;
            }
        }

        protected bool Initialize()
        {
            // initialize members
            if (_transform == null)
            {
                _transform = transform;
            }

            // prepare particle system
            ParticleSystemRenderer renderer = GetComponent<ParticleSystemRenderer>();
            var setParticleSystemMaterial = false;

            if (_particleSystem == null)
            {
                _particleSystem = GetComponent<ParticleSystem>();

                if (_particleSystem == null)
                {
                    return false;
                }

                // get current particle texture
                if (renderer == null)
                {
                    renderer = _particleSystem.gameObject.AddComponent<ParticleSystemRenderer>();
                }

                var currentMaterial = renderer.sharedMaterial;
                if (currentMaterial && currentMaterial.HasProperty("_MainTex"))
                {
                    particleTexture = currentMaterial.mainTexture;
                }

                // automatically set scaling
                var main = _particleSystem.main;
                main.scalingMode = ParticleSystemScalingMode.Local;

                _particles = null;
                setParticleSystemMaterial = true;
            }
            else
            {
                if (Application.isPlaying)
                {
                    setParticleSystemMaterial = (renderer.material == null);
                }
#if UNITY_EDITOR
                else
                {
                    setParticleSystemMaterial = (renderer.sharedMaterial == null);
                }
#endif
            }

            // automatically set material to UI/Particles/Hidden shader, and get previous texture
            if (setParticleSystemMaterial)
            {
                var material = new Material(Shader.Find("UI/Particles/Hidden"));
                if (Application.isPlaying)
                {
                    renderer.material = material;
                }
#if UNITY_EDITOR
                else
                {
                    material.hideFlags = HideFlags.DontSave;
                    renderer.sharedMaterial = material;
                }
#endif
            }

            // prepare particles array
            if (_particles == null)
            {
                _particles = new ParticleSystem.Particle[_particleSystem.main.maxParticles];
            }

            // prepare uvs
            if (particleTexture)
            {
                _uv = new Vector4(0, 0, 1, 1);
            }
            else if (particleSprite)
            {
                _uv = Sprites.DataUtility.GetOuterUV(particleSprite);
            }

            // prepare texture sheet animation
            _textureSheetAnimation = _particleSystem.textureSheetAnimation;

            _textureSheetAnimationFrames = 0;
            _textureSheedAnimationFrameSize = Vector2.zero;
            if (_textureSheetAnimation.enabled)
            {
                _textureSheetAnimationFrames = _textureSheetAnimation.numTilesX * _textureSheetAnimation.numTilesY;
                _textureSheedAnimationFrameSize = new Vector2(1f / _textureSheetAnimation.numTilesX,
                    1f / _textureSheetAnimation.numTilesY);
            }

            _renderer = renderer;

            return true;
        }

        protected override void Awake()
        {
            base.Awake();

            if (!Initialize())
            {
                enabled = false;
            }
        }

        private UIVertex[] GenerateStretchedMesh(Color32 meshColor, Vector2 size, Vector2 position, float rotation,
            Vector4 uv, int particleId)
        {
            //print(size);
            var velocity = _particles[particleId].totalVelocity;
            var scalefactor = CalculateScale(velocity);

            var width = size.x;
            var height = CalculateHeight(size, velocity);

            var quad = QuadUtils.GenerateModel(width, height, scalefactor, meshColor, uv);
            var finalAngle = CalculateAngle(velocity, rotation);

            quad = QuadUtils.ApplyPositionAndRotationTransform(quad, position, finalAngle);

            return quad;
        }

        private float CalculateHeight(Vector2 size, Vector3 velocity)
        {
            var stretched = _renderer.lengthScale;
            var velocityStretched = _renderer.velocityScale;
            var stretchedScale = stretched + velocityStretched * velocity.magnitude;

            //print(stretched + ", " + stretchedScale + ", " + size);
            
            return stretchedScale * size.y;
        }

        private float CalculateScale(Vector3 velocity)
        {
            var cubeX = velocity.x * velocity.x;
            var cubeY = velocity.y * velocity.y;

            var v3M = velocity.magnitude;
            var v2M = Math.Sqrt(cubeX + cubeY);

            var scalefactor = (float) v2M / v3M;

            return scalefactor;
        }

        private static float CalculateAngle(Vector3 velocity, float rotation)
        {
            var up = Vector2.up;
            var velocity2 = new Vector2(velocity.x, velocity.y);
            var angle = Vector2.Angle(velocity2, up);
            var finalAngle = velocity2.x > 0 ? -angle : angle;

            finalAngle *= Mathf.Deg2Rad;
            finalAngle += rotation;

            return finalAngle;
        }

        private Vector4 CalculateUv(Vector4 uv, ParticleSystem.Particle particle)
        {
            var result = uv;
            if (_textureSheetAnimation.enabled)
            {
                var frameProgress = 1 - (particle.remainingLifetime / particle.startLifetime);

                if (_textureSheetAnimation.frameOverTime.curveMin != null)
                {
                    frameProgress =
                        _textureSheetAnimation.frameOverTime.curveMin.Evaluate(
                            1 - (particle.remainingLifetime / particle.startLifetime));
                }
                else if (_textureSheetAnimation.frameOverTime.curve != null)
                {
                    frameProgress = _textureSheetAnimation.frameOverTime.curve.Evaluate(frameProgress);
                }
                else if (_textureSheetAnimation.frameOverTime.constant > 0)
                {
                    frameProgress = _textureSheetAnimation.frameOverTime.constant -
                                    (particle.remainingLifetime / particle.startLifetime);
                }

                var frame = 0;

                switch (_textureSheetAnimation.animation)
                {
                    case ParticleSystemAnimationType.WholeSheet:
                        frame = Mathf.CeilToInt(frameProgress * _textureSheetAnimationFrames) - 1;
                        break;

                    case ParticleSystemAnimationType.SingleRow:
                        frame = Mathf.FloorToInt(frameProgress * _textureSheetAnimation.numTilesX);

                        var row = _textureSheetAnimation.rowIndex;
#if UNITY_5_5_OR_NEWER
                        if (_textureSheetAnimation.useRandomRow)
                        {
                            Random.InitState((int) particle.randomSeed);
                            row = Random.Range(0, _textureSheetAnimation.numTilesY);
                        }
#endif
                        frame += row * _textureSheetAnimation.numTilesX;
                        break;
                }

                frame %= _textureSheetAnimationFrames;
                var maxY = 1 - _textureSheedAnimationFrameSize.y;
                result.x = frame % _textureSheetAnimation.numTilesX * _textureSheedAnimationFrameSize.x;
                //particleUV.y = Mathf.FloorToInt(frame / _textureSheetAnimation.numTilesX) * _textureSheedAnimationFrameSize.y;
                // ReSharper disable once PossibleLossOfFraction
                result.y = maxY - frame / _textureSheetAnimation.numTilesX * _textureSheedAnimationFrameSize.y;
                result.z = result.x + _textureSheedAnimationFrameSize.x;
                result.w = result.y + _textureSheedAnimationFrameSize.y;
                //print(string.Format("{4}: {0}, {1}, {2}, {3}", particleUV.x, particleUV.y, particleUV.z, particleUV.w, frame));
            }

            return result;
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                if (!Initialize())
                {
                    return;
                }
            }
#endif
            vh.Clear();

            if (!gameObject.activeInHierarchy)
            {
                return;
            }
            
            var count = _particleSystem.GetParticles(_particles);

            for (var i = 0; i < count; ++i)
            {
                var particle = _particles[i];
                Vector2 position = (_particleSystem.main.simulationSpace == ParticleSystemSimulationSpace.Local
                    ? particle.position
                    : _transform.InverseTransformPoint(particle.position));


                var rotation = -particle.rotation * Mathf.Deg2Rad;

                var particleColor = particle.GetCurrentColor(_particleSystem);
                //var size = particle.GetCurrentSize(_particleSystem);
                var size = particle.GetCurrentSize3D(_particleSystem) * ScaleAmount;

                // apply scale
                if (_particleSystem.main.scalingMode == ParticleSystemScalingMode.Shape)
                {
                    position /= canvas.scaleFactor;
                }

                // apply texture sheet animation
                var particleUv = CalculateUv(_uv, particle);

                UIVertex[] quad;
                if (_renderer.renderMode == ParticleSystemRenderMode.Stretch)
                {
                    quad = GenerateStretchedMesh(particleColor, size, position, rotation, particleUv, i);
                }
                else if(Math.Abs(rotation) < Tolerance)
                {
                    quad = QuadUtils.GetBillboardNotRotatedQuad(position, size, particleColor, particleUv);
                }
                else
                {
                    quad = QuadUtils.GetBillboardRotatedQuad(position, rotation, size, particleColor, particleUv);
                }

                vh.AddUIVertexQuad(quad);
            }
        }

        private void Update()
        {
            if (Application.isPlaying)
            {
                // unscaled animation within UI
                _particleSystem.Simulate(Time.unscaledDeltaTime, false, false);
                SetAllDirty();
            }
        }

#if UNITY_EDITOR
        private void LateUpdate()
        {
            if (!Application.isPlaying)
            {
                SetAllDirty();
            }
        }
#endif
    }
#endif
}