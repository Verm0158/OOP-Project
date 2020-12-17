/// <reference path="entry.ts"/>
///<reference path="Bird.ts"/>
/**
 * e.g to draw a "tube" and moving it horizontally:
 *
 * const tube = new PIXI.Graphics();
 * tube.beginFill(0xffffff);
 * //drawRect(x,y,width,height)
 * tube.drawRect(100,0,50,120);
 * tube.drawRect(100,200,50,512);
 * tube.endFill():
 *
 * tube.x = 200;
 * stage.addChild(tube);
 * renderer.render(stage);
 */

class Tube {
    private x: number;
    private y: number;
    private innerDistance = 80;
    private tubeWidth = 20;
    
    private sprite = new PIXI.Graphics();
    
    constructor(stage: PIXI.Container, x: number) {
        stage.addChild(this.sprite);
        this.reset(x);
    }
    
    /**
     * in the Reset, we use X and Y to control position.
     * if you have the speedX and speedY, you can also update bird's flying
     * orientation by:
     *
     * this.sprite.rotation=Math.atan(speedY / speedX)
     * @param {number} x
     */
    reset(x: number = canvasWidthHeight + 20) {
        this.x = x;
        
        const tubeMinHeight = 60;
        const randomNum = Math.random() * (canvasWidthHeight - 2 * tubeMinHeight - this.innerDistance);
        this.y = tubeMinHeight + randomNum;
    }
    
    /**
     * after we have moving objects (tubes and bird) we can treat them as rectangles.
     *
     * if we have their topmost point, width and height(x1,y1,w1,h1 and x2,y2,w2,h2)
     * then we can check whether two rectangles are hitting each other:
     *
     * if ((x1+w1<x2) || (x2+w2<x1) || (y1+h1<y2) ||y2+h2<y1)) no collision
     * @param {number} x
     * @param {number} y
     * @param {number} width
     * @param {number} height
     * @returns {boolean}
     */
    checkCollision(x: number, y: number, width: number, height: number) {
        if (!(x + width < this.x || this.x + this.tubeWidth < x || this.y < y)) {
            return true;
        }
        return !(x + width < this.x || this.x + this.tubeWidth < x || y + height < this.y + this.innerDistance);
        
    }
    
    update() {
        this.x -= GAME_SPEED_X / 60;
        if (this.x < -this.tubeWidth) this.reset();
        
        this.sprite.clear();
        this.sprite.beginFill(0xffffff, 1);
        const { x, y, tubeWidth, innerDistance } = this;
        this.sprite.drawRect(x, 0, tubeWidth, y);
        this.sprite.drawRect(x, y + innerDistance, tubeWidth, canvasWidthHeight);
        this.sprite.endFill();
    }
}


