using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Interface
    {
        private float lifebarlenght;
        private float fuelspent;
        private bool isExplosiveWeaponAvailable;
        private bool isNormalWeaponSet;
        private float lifebarScale = 0.25f;
        private float fuelbarScale = 0f;

        public float Lifebarlenght { get => lifebarlenght; set => lifebarlenght = value; }
        public float Fuelspent { get => fuelspent; set => fuelspent = value; }
        public bool IsExplosiveWeaponAvailable { get => isExplosiveWeaponAvailable; set => isExplosiveWeaponAvailable = value; }
        public bool IsNormalWeaponSet { get => isNormalWeaponSet; set => isNormalWeaponSet = value; }

        public Interface()
        {
            lifebarlenght = 500;
            fuelspent = 0;
            isExplosiveWeaponAvailable = false;
            isNormalWeaponSet = true;
        }
        public void Render()
        {
            Engine.Draw("Textures/UI/UserInterface.png", 0, 500, 0.25f, 0.25f, 0, 0, 0);
            Engine.Draw("Textures/UI/LifeBar.png", 0, 500, lifebarScale, 0.25f, 0, 0, 0);
            Engine.Draw("Textures/UI/LifeBarMarco.png", 0, 500, 0.25f, 0.25f, 0, 0, 0);
            Engine.Draw("Textures/UI/FuelBar.png", 0, 500, 0.25f, 0.25f, 0, 0, 0);
            Engine.Draw("Textures/UI/FuelBarBlack.png", 0, 500, 0.25f, fuelbarScale, 0, 0, 0);
            Engine.Draw("Textures/UI/FuelBarMarco.png", 0, 500, 0.25f, 0.25f, 0, 0, 0);
            if (IsExplosiveWeaponAvailable)
            {
                if (IsNormalWeaponSet)
                {
                    Engine.Draw("Textures/UI/NormalGunOn.png", 0, 500, 0.25f, 0.25f, 0, 0, 0);
                    Engine.Draw("Textures/UI/ExplosiveGunOff.png", 0, 500, 0.25f, 0.25f, 0, 0, 0);
                }
                else
                {
                    Engine.Draw("Textures/UI/NormalGunOff.png", 0, 500, 0.25f, 0.25f, 0, 0, 0);
                    Engine.Draw("Textures/UI/ExplosiveGunOn.png", 0, 500, 0.25f, 0.25f, 0, 0, 0);
                }    
            }
            else
            {
                Engine.Draw("Textures/UI/NormalGunOn.png", 0, 500, 0.25f, 0.25f, 0, 0, 0);
            }    
            
        }
        public void Update()
        {
            CalculateFuelBarScale();
            CalculateLifeBarScale();
        }
        public void CalculateLifeBarScale()
        {
            lifebarScale = (Lifebarlenght / 500) * 0.25f;
        }
        public void CalculateFuelBarScale()
        {
            fuelbarScale = Fuelspent * 0.25f / 350.0f;
        }
    }
}
