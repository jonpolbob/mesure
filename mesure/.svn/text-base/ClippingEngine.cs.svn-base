/*
 * \author nc
 * \date 01 04 09
 * 
 * 
 * 
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Diagnostics;

namespace mesure
{
    /// <summary>
    /// ClippinEngine class
    /// 
    /// </summary>
    public class ClippingEngine
    { //--------------------------------------------------------
        private Rectangle rSourceRect; // rectangle source complete
        private Rectangle rSrcClipRect; // sous rectangle source
        private Rectangle rVisuRect; // rectangel destination coords client
        private Rectangle rParentRect; // rectangle zone cleint du parent
        private Rectangle rDefClipRect = new Rectangle(0, 0, 640, 480); // rectangl par dfaut
         private bool SrcRectOK = false;
        //--------------------------------------------------------
        
       /// <summary>
        /// get / set la taille totale image source
        /// init le clipp et la visu a la meme taille
        /// </summary>
        public Rectangle  SourceRect
        {
            set
            {
                SrcRectOK = true;
                rSrcClipRect = _SetSourceRect(value.Width, value.Height); // positionne sourcerect
                processChangeClipRect(rSrcClipRect);  // rend le cliprect egal au sourcerect et fait un calcvisusize
              }
            get { return rSourceRect; }
        }



        /// <summary>
        /// get de la zone clippee dans la source
        /// </summary>
        public Rectangle ClipRect
        { get {
                if (SrcRectOK)
                    return rSrcClipRect; 
                else
                    return rDefClipRect;
              } 
        }

                
        /// <summary>
        /// renvoie la position de la fenetre de visualisation de la video 
        /// dans son panel de visualisation
        /// </summary>
        public Point VisuLocation
        {
            get { return new Point(rVisuRect.Left, rVisuRect.Top); }
        }


       
        /// <summary>
        /// get de la size de la fenetre destination (visu)
        /// </summary>
        public Size VisuSize
        {get {return new Size(rVisuRect.Width,rVisuRect.Height);}
        }

        
       
        /// <summary>
        /// recalcule le parametre rourcerect a partir de width et height recu
        /// ne modifie rien d'autre
        /// </summary>
        /// <param name="Width">nouvelle width de la source</param>
        /// <param name="Height">nouvelle height de la source</param>
        /// <returns>renvoie le sourcerect remis a jour</returns>
        private Rectangle _SetSourceRect(int Width, int Height)
        {
            rSourceRect.X  = 0;
            rSourceRect.Y = 0;
            rSourceRect.Width= Width;
            rSourceRect.Height = Height;

            return rSourceRect;
        }


        /// <summary>
        /// recalcule les memebres du clipper qd la taille de la fenete parent est modifiee
        /// dstrect est recalcule et parentrect est mis a jour
        /// suivant le mode de zoom, les reaction sont diffierentes
        /// par contre, on ne renvoie que la size de la fenetre car e sera le parent qui la positionnera, selon les rulers.
        /// </summary>
        /// <param name="newvisu">dimension de la feneter parent devant recevoir la visu</param>
        /// <returns></returns>
        public Size processChangeVisuRect(Rectangle newParentRect)
        {lock (this)
            {

             Size visusize = _RecalcVisuSize(rSrcClipRect, newParentRect); // taille du cadre video
             Debug.Write("par w =" + newParentRect.Width + "h=" + newParentRect.Height + "vid w = " + visusize.Width + "-h=" + visusize.Height + "\r\n");
             rVisuRect.Width = visusize.Width;
             rVisuRect.Height = visusize.Height;
             //rParentRect = newParentRect;
             rParentRect = newParentRect;
            
             return visusize;
            }
        }   


        /// <summary>
        /// recalcule les memebres dstrect et modifie cliprect 
        /// quand le on demande un changement de cliprect (zoom, changement taille video etc...)
        /// ca arrive quand on change de camera, ou ca arrivera quand on fera zoom
        /// ca modifie aussi visurect qui est recalculé
        /// </summary>
        /// <param name="newclip">nouveau rectangle de cllip</param>
        /// <returns>renvoie le visusize</returns>
        public Size processChangeClipRect(Rectangle newclip)
        {lock (this)
            {
                Size size;
                size = _RecalcVisuSize(newclip, rParentRect); // calcul de la position possible pour cette fenetre

                rVisuRect.Width = size.Width; 
                rVisuRect.Height = size.Height;
                rSrcClipRect= newclip;

                return size;
            }
        }

        
        /// <summary>
        /// convertit un point client en point source
        /// </summary>
        /// <param name="visuCoordsPt">point en coordonnees visu</param>
        /// <returns>point en coordonnees source</returns>
        public PointF convVisuToSrc(PointF visuCoordsPt)
        {
            double xcli = visuCoordsPt.X;
            double ycli = visuCoordsPt.Y;

            double percentx = xcli/rVisuRect.Width ; // coor relative en coord cli
            
            double percenty = ycli/rVisuRect.Height; // coor relative en coord cli

            double srcx = this.rSrcClipRect.Width * percentx + this.rSrcClipRect.X;
            double srcy = this.rSrcClipRect.Height* percenty + this.rSrcClipRect.Y;

            PointF pointout = new PointF((float)srcx, (float)srcy);
            return pointout;
        }
        
        
        /// <summary>
        /// conversion coordonnees source -> coordonnees visu
        /// </summary>
        /// <param name="srcCoordsPt"></param>
        /// <returns></returns>
        public PointF convSrcToVisu(PointF srcCoordsPt)
        {double srcx = srcCoordsPt.X;
         double srcy = srcCoordsPt.Y;

         // pour l'instant le clipping est en origine 0,0
         double percentx = srcx / rSrcClipRect.Width; // coor relative en coord cli
         double percenty = srcy / rSrcClipRect.Height; // coor relative en coord cli

         double visux = rVisuRect.Width * percentx;
         double visuy = rVisuRect.Height* percenty;

         return new PointF((float)visux, (float)visuy);
        }

        /// <summary>
        /// renvoie la position du rectangle de visualisation en fonction de la zone client et de la taille du cliprect
        /// </summary>
        /// <param name="cliprect">rectangle du clipping</param>
        /// <param name="ParentRect">rectangle du parent</param>
        /// <returns></returns>
        private Size _RecalcVisuSize(Rectangle clipRect, Rectangle ParentRect)
        {// si pas de cam : on dessine sur toute al fenetre
         Size pictsize; // taille de l'image a afficher
         pictsize = new Size(ParentRect.Width,ParentRect.Height );
         if (clipRect.Height == 0)
             return pictsize;

         // facteur ecran clipper
         double clifact = clipRect.Width / (double)clipRect.Height;

         // facteur zone client de parent
         double visufact;
         if (ParentRect.Height != 0)
            visufact = ParentRect.Width / (double)ParentRect.Height;
         else
            visufact = 0;

         // calcul du rectangle de dessin de l'image
         if (clifact > visufact) // l'imag est plus allongee en largeur que la visu
             {// la fenetre de viu aura la largeur de la video
              // la video n'occupera pas tout la hauteur
              pictsize.Width = ParentRect.Width;
              pictsize.Height = (int)(ParentRect.Width / clifact);
             }
          else
             {// la visu a la meme hauteur que la video
              // mais occupera pas tout la largeur
              pictsize.Height= ParentRect.Height;
              pictsize.Width = (int)(ParentRect.Height * clifact);
             }
                
            return pictsize; // nouvelel valeur dur ectangle de visu
        }


    }
}

