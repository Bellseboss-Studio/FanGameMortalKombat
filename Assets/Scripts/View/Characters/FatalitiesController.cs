using System.Collections.Generic;
using UnityEngine;

namespace View.Characters
{
    public class FatalitiesController : MonoBehaviour
    {
        public Dictionary<string, Queue<string>> ListadoDeSecuencias { get; private set; }
    
        public FatalitiesController()
        {
            //agregamos la secuencia de fireball
            ListadoDeSecuencias = new Dictionary<string, Queue<string>>
            {
                { SecuenciasPermitidas.FIREBALL, CrearSecuenciaFireBall() }
            };
        }
    
        private Queue<string> CrearSecuenciaFireBall()
        {
            Queue<string> fireBall = new Queue<string>();
            fireBall.Enqueue(SecuenciasPermitidas.ABAJO);
            fireBall.Enqueue(SecuenciasPermitidas.DIAGONALDELANTE);
            fireBall.Enqueue(SecuenciasPermitidas.DELANTE);
            fireBall.Enqueue(SecuenciasPermitidas.PUNIODEBIL);
            return fireBall;
        }
    }
}