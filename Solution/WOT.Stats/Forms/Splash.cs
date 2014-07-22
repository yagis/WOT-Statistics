using System;
using System.Threading;
using System.Windows.Forms;

namespace WOT.Stats
{
    public partial class SplashScreen_old : Form
    {
         // Threading
        static SplashScreen ms_frmSplash = null;
           static Thread ms_oThread = null;
   
           // Fade in and out.
           private double m_dblOpacityIncrement = .05;
           private double m_dblOpacityDecrement = .08;
           private const int TIMER_INTERVAL = 50;

           private System.Windows.Forms.Timer timer1;

    
        public SplashScreen()
        {
            InitializeComponent();
            this.Opacity = .00;
            timer1.Interval = TIMER_INTERVAL;
            timer1.Start();
        }

         static public void ShowSplashScreen()
          {
              // Make sure it's only launched once.
              if( ms_frmSplash != null )
                  return;
              ms_oThread = new Thread(SplashScreen.ShowForm) { IsBackground = true, ApartmentState = ApartmentState.STA };
              ms_oThread.Start();
          }

        static public SplashScreen SplashForm 
          {
              get
              {
                  return ms_frmSplash;
              } 
          }

        static private void ShowForm()
          {
              ms_frmSplash = new SplashScreen();
              Application.Run(ms_frmSplash);
          }

        static public void CloseForm()
          {
              if( ms_frmSplash != null && ms_frmSplash.IsDisposed == false )
              {
                  // Make it start going away.
                  ms_frmSplash.m_dblOpacityIncrement = - ms_frmSplash.m_dblOpacityDecrement;
              }
              ms_oThread = null;  // we don't need these any more.
              ms_frmSplash = null;
              
          }
  
          // Static method called from the initializing application to 
          // give the splash screen reference points.  Not needed if
          // you are using a lot of status strings.
          static public void SetReferencePoint()
          {
              if( ms_frmSplash == null )
                  return;
          }

          //********* Event Handlers ************
  
          // Tick Event handler for the Timer control.  Handle fade in and fade out.  Also
          // handle the smoothed progress bar.
          private void timer1_Tick(object sender, System.EventArgs e)
          {  
              if( m_dblOpacityIncrement > 0 )
              {
                  if( this.Opacity < 1 )
                      this.Opacity += m_dblOpacityIncrement;
              }
              else
              {
                  if( this.Opacity > 0 )
                      this.Opacity += m_dblOpacityIncrement;
                  else
                  {
                      this.Close();
                  }
              }
          } 
      
  
      private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
