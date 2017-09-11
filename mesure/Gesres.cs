using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace mesure
{
    /// <summary>
    /// types de colonne
    /// variable : sans type particulier
    /// legendedeb : la colonne est en debut de ligne
    /// legendefin : la colonne doit ariver apres lesresultats
    /// result : la colonne fait partie du group de resultats
    /// si on fait un add avec -1 comme numero de colonne :
    /// une colonne legendedeb sera rajoutee apres la derniere legendedeb
    /// une colonne legendefin sera rajoutee apres la derniere legende
    /// une colonne result sera apres laderniere colonne resultat
    /// </summary>
    public enum coltype { variable, result, legendedeb, legendefin };

   // public delegate event gesresHandle(object src, int action);

    /// <summary>
    /// gestionnaire de résultats
    ///  gere les resultats en foramt variable : chaque resultat est stocké avec le nom de sa colonne et sn numero de ligne
    /// si une nouvelle colonne apparait, c'est capable de regenerer tout le dispositif
    /// chaque colonne est designee dans la clase ar son id, comme ca si on decale une colonne tous les resultats restent correctemet attaches a elle
    /// </summary>
    public class Gesres
    {int m_curnbligne =0; // ligne courante en ecriture
     resLigne m_curligneecr = null; // ligne dans laqeulle on travaille actuellement
     int m_curcolread = 0;
     int m_curlineread = 0;

     int m_curID =0; // id unique d'identification des colonne

     ArrayList m_listcolonnes = new ArrayList();
     ArrayList m_listligne = new ArrayList();

     // SOUSclasse d'elements de ligne de titre de colonne
    public class colonnedescr
    { public colonnedescr(){}
      public int uniqueID; // id permettant de toujours retrouver ue colonne quelle que soit sa position
      public string nom;
      public coltype m_type;
    }
    
    /// <summary>
    /// SOUSCLASS DE gstion d'une case du tableaude resultats
    /// </summary>
    public class resCase
    {
        public int idcolonne; // id  de la legende de la colonne
        public string resultat;

    }

    
    /// <summary>
    /// une ligne de resultats 
    /// </summary>
    public class resLigne
    {
        public int m_numligne; // numero de ligne
        public ArrayList m_listcases = new ArrayList();        
    }

    /// <summary>
    /// renvoie un I dde colonne disponible
    /// </summary>
    /// <returns></returns>
    private int getnewcolID()
    {return ++m_curID ;
    }

    

    /// <summary>
    /// prepare une nouvelle ligne de resultats
    /// et la rajoute au tableau des resultats
    /// </summary>
    /// <param name="numligne"></param>
    /// <returns></returns>
    public resLigne AddNewLigne()
    {// la ligne n'existe pas deja
     m_curligneecr = new resLigne();
     m_curligneecr.m_numligne = m_curnbligne++; // numero de ligne commence a 0
     m_listligne.Add(m_curligneecr); // ajoute cette ligne au tableau
     return m_curligneecr;
    }       

    
    /// <summary>
    /// renvoie la ligne en cours d'ecriture      
    /// </summary>
    /// <returns></returns>
    public resLigne getLigneEcr()
    {return m_curligneecr;
    }
    
    /// <summary>
    /// vire la derniere ligne de resultats
    /// </summary>
    /// <returns></returns>
    public int RemoveLigne() 
    {if (m_listligne.Count ==0)
        return 1;
     
     resLigne derligne = (resLigne)m_listligne[m_listligne.Count - 1];
     derligne.m_listcases.Clear(); // on vide la ligne
     m_listligne.Remove(derligne); // on vire la derniere ligne
     if (m_listligne.Count != 0)
         m_curligneecr = (resLigne)m_listligne[m_listligne.Count - 1];
     else
         m_curligneecr = null;
     // declencher un evenement de redessin
     m_curnbligne = m_listligne.Count;
     return 0;
    }       

     /// <summary>
     /// ajoute une celleule a la ligne d'ecriture courante
     /// avec cette colonne et cette valeur
     /// </summary>
     /// <param name="nomcol">nom de la colonne</param>
     /// <param name="valeur">valeur a enregistrer (correctemetn formattée)</param>
     /// <returns></returns>
     public int AddCell(string nomcol, string valeur)
     {int idcol = getidcolonne(nomcol); // id de la colonne
      if (idcol != 0) // colonne trouvee
        {//il faudrait verifier que cette ligne n'a pas deja un resultat avec cette colonne...
          // dans la pratique actuellement c'est impossible
          resCase newcase = new resCase();
          newcase.resultat = valeur;
          newcase.idcolonne = idcol;
          m_curligneecr.m_listcases.Add(newcase);
        }
      
      return 0;

     }

        /// <summary>
        /// renvoie l'index de la derniere colonne de ce type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private int getLstColType(coltype type)
        {int lstcol =-1;
            int numcol =0;

        foreach (colonnedescr colon in m_listcolonnes)
        {
         if (colon.m_type == type)
                lstcol = numcol;
            numcol++;
            }
            
         return lstcol;
        }

        /// <summary>
        /// efface du tableau de resultats la colonne de nom nomcolonne
        /// et vire tous les resultatas
        /// en fait ce serai mieux de pas les virer, juste enlever la colonne        
        /// </summary>
        /// <param name="nomcolonne">nom de la colonne a enlever</param>
        /// <returns>1 si col pas trouvee, 0 si OK</returns>
        public int DelColonne(string nomcolonne)
        {// on regarde le ID de cette colonne pour la virer des resultats 
            int ID = getidcolonne(nomcolonne);
            if (ID == 0)
                return 1; // la col existe pas

            // fina lement on fait que enlever la colonne de l'en tete, si elle revient les reultats retrouveront leur colonne, 
            // et ceux qui en ont pas aurtaont une ligne  vide sans probleme
            /// pour pouvoir faire un undo
            /// pour l'instant on la vire simplement
            /*foreach (resLigne derligne in this.m_listligne)
                foreach (resCase curcase in derligne.m_listcases)
                    if (curcase.idcolonne == ID)
                    {
                        derligne.m_listcases.Remove(curcase);
                        break;
                    }
             * */

            // on vire la colonne de l'en tete
            foreach (colonnedescr colon in m_listcolonnes)
                if (colon.uniqueID == ID)
                    {
                    m_listcolonnes.Remove(colon);
                    break;
                    }

            return 0; // ok
        }
     


     /// <summary>
     /// rajoute une colonne position(1ere position = 1)
     /// decale les colonnes suivantes
     /// cette gestion des colonnes ne concerne que les resultats
     /// les colonnes parametres et objectifs sont gerees séparament
     /// </summary>
     /// <param name="position">si -1 : positionnement automatique selon le type</param>
     /// <param name="type">type de colonne</param>
     /// <param name="nomcolonne">nom de la nouvelle colonne</param>
     /// <returns></returns>
     public int AddColonne(int position, coltype type, string nomcolonne)
     {// la liste des positions ne gere 
     // o commence par verifier que la colonne existe pas deja
      if (getidcolonne(nomcolonne) != 0)
          return 1; // la col existe deja

         // positionnement automatique: suivant le type de colonne : 3 strategies
      if (position == -1)
      {
          position = getLstColType(type);
          if (position == -1)
          {
              switch (type)
              {
                  case coltype.legendedeb:
                      position = 0; // legende de debut : pas trouve de colonne de lenede deb = on met dans 1ere colonne
                      break;
                  case coltype.legendefin:  // legende de fin : c'est la derniere colonne
                      position = m_listcolonnes.Count;
                      break;
                  case coltype.result: // on recherche la fin des legendes de deb
                      position = getLstColType(coltype.legendedeb);
                      if (position == -1) // si pas de legende de type
                          position = 0; // on insere au debut
                      else
                          position += 1; // on inserera apres la derniere position 
                      break;
              }
          }
          else // on insere apres la derniere position renvoyee
              position += 1;

      }

     // si elle existe pas deja : on la rajoute
     colonnedescr nouvdescr = new colonnedescr();
     nouvdescr.nom = nomcolonne;
     nouvdescr.uniqueID = getnewcolID();
     nouvdescr.m_type = type;

      // on ajoute une colonne, ca peut deplacer tous les id des resultats
      m_listcolonnes.Insert(position,nouvdescr);
      //if (position != -1) // pas insertion en fin de tableau : oil faudra rafaichir l'affichage
      //    if (gesresHandle != null)
      //        gesresHandle(this, 1); // 1 : event colonne ajoutee
         
      return 0;
     }

     /// <summary>
     /// renvoi l'id de la colonne de nom nomcolonne
     /// </summary>
     /// <param name="nomcolonne"></param>
     /// <returns></returns>
     private int getidcolonne(string nomcolonne)
     {foreach (colonnedescr nouvdescr in m_listcolonnes)
         if (nouvdescr.nom.CompareTo(nomcolonne)==0)
             return nouvdescr.uniqueID; // trouvée
      
      return 0; // pas trouvée.
     }

     /// <summary>
     /// claer de tous les resultats du tableau
     /// </summary>
     /// <returns></returns>         
        public int ClearResul()
        {   do
            { } while (RemoveLigne() == 0); // on efface totes les lignes
            return 0;
    

         return 0;
        }
      


      // remet la tete de lecture des colonnes sur la premiere colonne et retourne le nb de cols
      public int ResetColRead()
      {m_curcolread =0;       
       return m_listcolonnes.Count; // nombre de colonnes
      }

      /// <summary>
      /// renvoie le titre de la prochaine colonne
      /// </summary>
      /// <returns></returns>
      public string NxtColRead()
      {colonnedescr descr;
       if (m_listcolonnes.Count > m_curcolread)
          {descr = (colonnedescr)m_listcolonnes[m_curcolread++];
           return descr.nom;
           }
          
       return null;
      }
      

      /// <summary>
      // revient au debut du tableau -premiere ligne - pour la lecture des donnees
      /// </summary>
      /// <returns></returns>
      public int ResetLinesRead()
      {m_curlineread =0;  
       return m_listligne.Count;
      }

        /// <summary>
        /// renvoie un chaine correcte de la ligne de resultat, corectemetn rangée selon les colonnes
        /// avec des vides pour les champs vides
        /// </summary>
        /// <param name="lineout"></param>
        /// <returns></returns>
      public int NxtLineRead(out string lineout)
      {
       
       foreach ( resLigne ligne in m_listligne)
        {if (ligne.m_numligne == m_curlineread) // on a trouvé la ligne
        {
            lineout = CoreSystem.Instance.ResCumul.readformattedline(ligne);
            m_curlineread++; // on passe a la ligne suivante            
            return 1; // on a trouve la ligne : on sort
            }
        }

       lineout = null;
       return 0;

      }

        
     /// <summary>
     /// sort la ligne de resutlats correctemetn formattee en fonction des cononnes
     /// a afficher
     /// </summary>
     /// <param name="line"></param>
     /// <returns></returns>
     public String readformattedline(resLigne line)
     {String txtout = "";
      string tabulstr = "\t";

      if (line == null)
         return null;

      foreach ( colonnedescr colonne in m_listcolonnes)
          {int curid = colonne.uniqueID; // id de al colonne a sortir
            bool found = false;

            foreach (resCase curcase in line.m_listcases)
             {if (curcase.idcolonne == curid) // on a un res pour cette colonne
                  {txtout += curcase.resultat + tabulstr; // on l'ajoute
                   found = true; // colonne trouvee
                   break;
                  }
             }

         if (!found) // on n'a pas trouve de res pour cette colonne
             txtout += tabulstr; //une tab en plus pour la colonne sans resultat
         }

      if (txtout.Length == 0)
          return txtout;
     
     // ona un tab en trop : on tronque la chaine
     return txtout.Remove(txtout.Length-1);
     }

      
    }    

}
