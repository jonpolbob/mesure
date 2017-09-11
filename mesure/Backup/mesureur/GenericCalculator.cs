using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace mesure
{
/// <summary>
/// interface permettant de faire deriver 
/// </summary>
    public interface IGenericCalculator 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <value> tdpanel devant etre utilise par ce calculateur pour ses dessins</value>
        TDPanel Panel
        { set;
        }

        void SetColor(Color newcolor);
        void repaint();
        
    }
    
    /// <summary>
    /// contient les elements comuns a tous les calculator
    /// il reste plus qu'a implementer l'interface
    /// </summary>
 public abstract class GenericCalculator : IGenericCalculator
    {
      public event MesureEventHandler VisuToSrcConv;
      public event MesureEventHandler SrcToVisuConv;
      public event MesureEventHandler SrcToEtalConv;
      public event ResuEventHandler ToResu;


     
     /// <summary>
      /// fonctions appelenat les events car on le peut pas appeler des events d'une classe mere 
     /// </summary>
     /// <param name="sender"></param>
     /// <param name="e"></param>
     protected void OnVisuToSrcConv(object sender, ref MesureEventArgs e)
     { if (VisuToSrcConv != null)
         VisuToSrcConv(sender, ref e);
     }

     protected void OnSrcToVisuConv(object sender, ref MesureEventArgs e)
     {if (SrcToVisuConv != null)
         SrcToVisuConv(sender, ref e);
     }


     protected void OnSrcToEtalConv(object sender, ref MesureEventArgs e)
     {if (SrcToEtalConv != null)
         SrcToEtalConv(sender, ref e);
     }

     protected void OnToResu(object sender, ref ResuEventArg e)
     {
         if (ToResu != null)
             ToResu(sender, ref e);
     }
           
     /// <summary>
     /// <value> couleur de trace</value>
     /// </summary>
     protected Color m_Color = Color.Red; 

        protected TDPanel lepanel;

              
        /// <summary>
        /// positionne la couleur de trace
        /// </summary>
        /// <param name="newcolor"></param>
        public void SetColor(Color newcolor)
        { m_Color = newcolor;
        this.repaint();
        }

        /// <summary>
        /// definit lepanel a utiliser 
        /// </summary>
        public TDPanel Panel
        {
            set { lepanel = value; }
        }
     
     public abstract void repaint();

    }

    
    
}
