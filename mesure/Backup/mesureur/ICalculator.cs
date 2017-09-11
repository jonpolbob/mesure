using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace mesure
{
    public delegate void ChgVisuSizeHandler(object sender, ref EventArgs e); ///< delegate de la fonction installee dans le handler de changement de taille de la fenetre video
    
    ///
    /// types pour l'etalonnage
    public enum etalflagstyp { none, distance, distancex, distancey, angle, surf, vol };
    
    /// <summary>
    /// interface ICalculator servant a definir les fonctions indispensables a un calculator, en plus des fonctions de base de genereic
    /// </summary>
    public interface ICalculator :IGenericCalculator // interface derive de la classe genereique .. est ce possible ?
{
    
        //TDPanel Panel { set;}
        
        // traite un changement de variable
        // renvoie 0 si resultat ok
        // renvoie 1 si attend encore qqchose        
        int OnChangeVariable(object sender, ChangeVariable e); ///< fonction executee quant la variable de UI a change 

        int SetActive(Control client); ///< rend ce calcul actif dans cette fenetre
        
        /// </summary>
        /// New result event - notify client about the new result
        /// </summary>
        // enregistrement ds parametres
        int SaveDisk(XMLAvElement elemnt);  // enregistremetn des parametres du mesureur sur disque
        int LoadDisk(XMLAvElement elemnt);  // lecture des parametres du mesureur sur disque       
        
        // resize de UI
        void OnChgVisuSize(object sender, EventArgs e); ///< appelé quand la fenetre video a change de taille
        
        // conversion des resultats
        event MesureEventHandler VisuToSrcConv; ///< conversion coordonnees visu vers source (pas toujours possible)
        event MesureEventHandler SrcToVisuConv; ///< conversion source vers coordonnees visu
        event MesureEventHandler SrcToEtalConv; ///< conversion source vers valeur etalonnee
        event ResuEventHandler ToResu; ///< event appele par le calculateur quand des mesures sont calculees (mouvement souris)
    
        // sortie des resultats
        double GetValeur(int ID, int unit); ///< renvoie la veleur de la mesure ID de ce calcul, mise en forme selon al valeur de unit passee
        string GetLegende(int ID); ///< renvoie la legende du resultat ID de ce calcul
        string GetUnit(int ID, ref etalflagstyp etalflags); ///< renvoie les infos sur l'unite de ce calcul de numero ID
        ArrayList GetListResu(); ///< renvoie la liste des id urtilises par ce calcul
    
    }

    /* public class ChgVisuSizeArg
     {
     }
    */
    
}
