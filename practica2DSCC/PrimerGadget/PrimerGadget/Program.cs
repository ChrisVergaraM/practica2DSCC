using System;
using System.Collections;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Controls;
using Microsoft.SPOT.Presentation.Media;
using Microsoft.SPOT.Presentation.Shapes;
using Microsoft.SPOT.Touch;

using Gadgeteer.Networking;
using GT = Gadgeteer;
using GTM = Gadgeteer.Modules;
using Gadgeteer.Modules.GHIElectronics;

namespace PrimerGadget
{   
    
    public partial class Program
    {
        GT.Picture foto = null;
        GT.StorageDevice dispositivo;
        GT.Timer timer;
        // This method is run when the mainboard is powered up or reset.   
        void ProgramStarted()
        {
           
            /*******************************************************************************************
            Modules added in the Program.gadgeteer designer view are used by typing 
            their name followed by a period, e.g.  button.  or  camera.
            
            Many modules generate useful events. Type +=<tab><tab> to add a handler to an event, e.g.:
                button.ButtonPressed +=<tab><tab>
            
            If you want to do something periodically, use a GT.Timer and handle its Tick event, e.g.:
                GT.Timer timer = new GT.Timer(1000); // every second (1000ms)
                timer.Tick +=<tab><tab>
                timer.Start();
            *******************************************************************************************/


            // Use Debug.Print to show messages in Visual Studio's "Output" window during debugging.
            Debug.Print("Program Started");
            camera.CameraConnected += camera_CameraConnected;
            camera.BitmapStreamed +=camera_BitmapStreamed;
            button.ButtonPressed += button_ButtonPressed;
            camera.PictureCaptured += camera_PictureCaptured;

            sdCard.Mounted += sdCard_Mounted;
           
            //camera.StopStreaming();
            //Timer para simular el reinicio de la cámara
            timer = new GT.Timer(1500,GT.Timer.BehaviorType.RunOnce); // every second (1000ms)
            timer.Tick += timer_Tick;
                
        }

        void timer_Tick(GT.Timer timer)
        {
            camera.StartStreaming();
        }

        void sdCard_Mounted(SDCard sender, GT.StorageDevice device)
        {
            Debug.Print("Tarjeta Montada");
        }

        void camera_PictureCaptured(Camera sender, GT.Picture e)
        {
            button.TurnLedOn();
            sdCard.StorageDevice.WriteFile("Foto.bmp", e.PictureData);

            button.TurnLedOff();
            timer.Start();
        }
        void button_ButtonPressed(Button sender, Button.ButtonState state)
        {
            camera.StopStreaming();            
            camera.TakePicture();
        }

        void camera_CameraConnected(Camera sender, EventArgs e)
        {
            // Camara conectada
            camera.StartStreaming();
        }



        void camera_BitmapStreamed(Camera sender, Bitmap e)
        {   
            //Funcion para visualizar la cámara
 	         displayT35.SimpleGraphics.DisplayImage(e,0,0);
        }
        


    }
}
