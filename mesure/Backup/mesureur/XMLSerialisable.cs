using System;
using System.Collections.Generic;
using System.Text;

namespace mesure.mesureur
{
    interface XMLSerialisable
    {
        int SaveDisk(XMLAvElement elemnt);  // enregistremetn des parametres du mesureur sur disque
        int LoadDisk(XMLAvElement elemnt);  // lecture des parametres du mesureur sur disque       
      
    }
}
