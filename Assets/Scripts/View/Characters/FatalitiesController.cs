using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

namespace View.Characters
{
    public class FatalitiesController : MonoBehaviour
    {
        [SerializeField] private PlayerCharacter player;
        [SerializeField] private PlayableTrack track;

        private Dictionary<string, Queue<string>> ListadoDeSecuencias { get; set; }

        private void Start()
        {
            player.fatality += Fatality;
            player.punio += Punio;
            player.patada += Patada;
            player.movimiento += Movimiento;
            //TimelineAsset.GetOutputTracks();
        }

        private void FixedUpdate()
        {
            //Aqui vamos a validar los movimientos
        }

        private void Movimiento(Vector2 vect)
        {
            
        }

        private void Patada()
        {
            
        }

        private void Punio()
        {
            
        }

        private void Fatality()
        {
            //si tiene enrgia o no para la fatality
            if (player.CanPlayFatality())
            {
                player.ResetEnergy();
                //ejecutar cinematica con Timeline
            }
        }

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


    public static class SecuenciasPermitidas
    {
        public const string ATRAS = "atras";
        public const string DIAGONALATRAS = "atrasabajo";
        public const string DELANTE = "delante";
        public const string DIAGONALDELANTE = "delanteabajo";
        public const string ABAJO = "abajo";
        public const string ARRIBA = "arriba";
        public const string PUNIOFUERTE = "punioFuerte";
        public const string PUNIODEBIL = "punioDebil";
        public const string PATADAFUERTE = "patadaFuerte";
        public const string PATADADEBIL = "patadaDebil";
        public const string VACIO = "";
        //poderes!!
        public const string FIREBALL = "fireball!!!!!!";
        public const string CORRER = "Correr";
        public const string SALTARHACIAATRAS = "Saltar hacia atras";
        public const string DRAGONPUNCH = "dragonPunch!!!";

    }
}