using RWCustom;
using UnityEngine;

namespace Items.ShoreCoco.Effects
{
    public class CocoWater : CosmeticSprite
    {
        private Vector2 startPos;
        private Vector2 vel;
        private float life;
        private float maxLife;
        private int drops;
        private Vector2[] dropPositions;
        private Vector2[] dropVelocities;
        private float[] dropLife;

        public CocoWater(Vector2 pos, Vector2 velocity)
        {
            this.pos = pos;
            this.lastPos = pos;
            this.startPos = pos;
            this.vel = velocity;
            this.maxLife = 60f; // 2 segundos a 30 FPS
            this.life = maxLife;
            
            // Crear múltiples gotas
            this.drops = 8;
            this.dropPositions = new Vector2[drops];
            this.dropVelocities = new Vector2[drops];
            this.dropLife = new float[drops];
            
            for (int i = 0; i < drops; i++)
            {
                dropPositions[i] = pos;
                dropVelocities[i] = velocity + Custom.RNV() * UnityEngine.Random.value * 5f;
                dropLife[i] = maxLife * (0.5f + UnityEngine.Random.value * 0.5f);
            }
        }

        public override void Update(bool eu)
        {
            base.Update(eu);
            
            life--;
            if (life <= 0f)
            {
                Destroy();
                return;
            }

            // Actualizar cada gota
            for (int i = 0; i < drops; i++)
            {
                if (dropLife[i] > 0f)
                {
                    dropLife[i]--;
                    dropPositions[i] += dropVelocities[i];
                    dropVelocities[i] *= 0.98f; // Fricción del aire
                    dropVelocities[i].y -= 0.9f; // Gravedad
                    
                    // Colisión con el suelo
                    if (room.GetTile(dropPositions[i]).Solid)
                    {
                        dropLife[i] = 0f;
                        // Crear pequeño splash
                        if (UnityEngine.Random.value < 0.3f)
                        {
                            room.AddObject(new WaterDrip(dropPositions[i], Custom.RNV() * 2f, false));
                        }
                    }
                }
            }
        }

        public override void DrawSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam, float timeStacker, Vector2 camPos)
        {
            for (int i = 0; i < drops; i++)
            {
                if (dropLife[i] > 0f && i < sLeaser.sprites.Length)
                {
                    Vector2 drawPos = Vector2.Lerp(dropPositions[i], dropPositions[i], timeStacker);
                    sLeaser.sprites[i].x = drawPos.x - camPos.x;
                    sLeaser.sprites[i].y = drawPos.y - camPos.y;
                    
                    float alpha = dropLife[i] / maxLife;
                    sLeaser.sprites[i].color = new Color(0.8f, 0.9f, 1f, alpha * 0.8f);
                    sLeaser.sprites[i].scale = Mathf.Lerp(0.5f, 1.5f, alpha);
                }
            }
            base.DrawSprites(sLeaser, rCam, timeStacker, camPos);
        }

        public override void InitiateSprites(RoomCamera.SpriteLeaser sLeaser, RoomCamera rCam)
        {
            sLeaser.sprites = new FSprite[drops];
            for (int i = 0; i < drops; i++)
            {
                sLeaser.sprites[i] = new FSprite("pixel", true);
                sLeaser.sprites[i].scale = 2f;
            }
            AddToContainer(sLeaser, rCam, null);
        }
    }
}