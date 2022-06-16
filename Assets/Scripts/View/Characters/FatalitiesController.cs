using System.Collections.Generic;

namespace View.Characters
{
    public class FatalitiesController
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