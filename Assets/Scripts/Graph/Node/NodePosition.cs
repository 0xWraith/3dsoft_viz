using UnityEngine;


namespace Softviz.Graph
{
    public class NodePosition : MonoBehaviour
    {
        private Vector3 desiredposition;
        private float Treshold = 0.1f;

        void Update()
        {
            // Používa sa tu nejaký vzorec, ktorý funguje približne tak, že sa od väčších po menšie a menšie kúsky približujeme k danej pozícii. 
            // Problém je, že sa nikdy táto nová pozícia nedosiahne, pretože sa takto ide do nekonečna. To sa vyriešilo tak, že ak je rozdiel 
            // medzi požadovanou pozíciou a aktuálnou pozíciou menší ako naša pomocná hodnota treshold (čiže ak sme dostatočne blízko), tak sa
            // uzlu rovno nastaví požadovaná pozícia.

            if (desiredposition != transform.localPosition)
            {
                Vector3 velocity = Vector3.zero;
                transform.localPosition = Vector3.SmoothDamp(transform.localPosition, desiredposition, ref velocity, 0.2f);
            }
            var v = desiredposition - transform.localPosition;
            if (v.magnitude < Treshold)
            {
                transform.localPosition = desiredposition;
            }
        }

        public void SetPosition(Vector3 position)
        {
            desiredposition = position;
        }
    }
}