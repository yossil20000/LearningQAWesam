'use strict';
var canvas = document.getElementById('can');
/* UFO*/
   class Ufo {
      constructor() {
         this.x = 77,
         this.y = 77,
         this.velocity = {
            x: 3,
            y: 3
         }
      }
      
      draw(c) {
         c.save()
         c.beginPath()
         c.arc(this.x, this.x, 50, 0, Math.PI * 2, false)
         c.fillStyle = "red";
         c.fill()
         c.closePath()
         c.restore()
      }
      
      update(c) {
         this.draw(c)
      }
   }

/* Canvas*/		

   class CanvasDisplay {
      constructor() {
         this.canvas = document.getElementById('can');
		   this.ctx = this.canvas.getContext('2d');
         this.stageConfig = {
		      width: window.innerWidth,
		      height: window.innerHeight
         };         
         this.canvas.width = this.stageConfig.width;
         this.canvas.height = this.stageConfig.height;
         this.Ufo = new Ufo();
      }
      
      animate() {
         this.ctx.clearRect(0, 0, this.stageConfig.width, this.stageConfig.height);
         this.Ufo.update(this.ctx)
      }
   }

function init()
{
    let canvasDisplay = new CanvasDisplay();
canvasDisplay.animate();
}
