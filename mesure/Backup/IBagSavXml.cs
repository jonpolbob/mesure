using System;
using System.Collections.Generic;
using System.Text;

namespace mesure
{
    public interface IBagSavXml
    {
     //  procedures de lecture ecriture dans le xml
     string getNomType(); //// renvoie le nom de ce type de bag
     string[] getTabVers(); /// RENVOIE LE TABLEAU decimal TOUTES LES VERSIONS DEPUIS LE DEBUT
        
     //  procedures de lecture ecriture dans le xml
        /// <summary>
        /// savedisk recoit un node
        /// ecrit dedans le type d'obket sauve et son numero de version
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
     int SaveDisk(XMLAvElement node); /// sauve le bag dans un element xml

     /// <summary>
     /// loaddisk  recoit un node
     /// lit dedans le type et verifie que c'est le bon -> exception badtype
     /// lit dedans un numero de version s'il y en a un et charge l'objet selon cette version -> exception errversion
     /// </summary>
     /// <param name="element">node contenant tout la sauvegarde</param>
     /// <returns></returns>
     int LoadDisk(XMLAvElement node); /// lit le bag dans un elemnt xml
     
    }
}
