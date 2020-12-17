/// <reference path="entry.ts"/>

const BIRD_FRAME_LIST = [
    './images/frame-1.png',
    './images/frame-2.png',
    './images/frame-3.png',
    './images/frame-4.png',
];

/**
 * now we have a bird and can apply different transformation to it; you can try
 * adding a new field speedY to BIRD class and update bird's position by
 * updating sprite's Y each Draw based on that field
 */
class Bird {
    private sprite = new PIXI.Sprite();
    private textureCounter: number = 0;
    
    constructor(stage: PIXI.Container) {
        stage.addChild(this.sprite);
        this.sprite.scale.x = 0.06;
        this.sprite.scale.y = 0.06;
        // Set the transformation origin
        this.sprite.anchor.set(0.5, 0.5);
        this.sprite.anchor.set(0.5, 0.5);
        this.reset();
        
        setInterval(this.updateTexture, 200);
    }
    
    private updateTexture = () => {
        this.sprite.texture = PIXI.loader.resources[BIRD_FRAME_LIST[this.textureCounter++]].texture;
        
        if (this.textureCounter === BIRD_FRAME_LIST.length) this.textureCounter = 0;
    }
    
    reset() {
        this.sprite.x = canvasWidthHeight / 6;
        this.sprite.y = canvasWidthHeight / 2.5;
    }
}

const renderer = PIXI.autoDetectRenderer(canvasWidthHeight, canvasWidthHeight, { backgroundColor: 0xc1c2c4 });
document.body.appendChild(renderer.view);
const stage = new PIXI.Container();

PIXI.loader
    .add(BIRD_FRAME_LIST)
    .load(setup);

let bird;
const button = document.querySelector('#start');
function setup() {
    bird = new bird(stage, tubeList, () => {
        // Called when bird hit tube/ground/upper bound
        gameFailed = true;
        button.classList.remove('hide');
        //TODO RESET AUTO LEVEL + SHOW SHOPVIEW
    });
    requestAnimationFrame(draw);
}

function draw() {
    if(gameStarted) {
        bird.updateSprite();
        if (!gameFailed) tubeList.forEach(d => d.update());
    }
    renderer.render(stage);
    requestAnimationFrame(draw);
}

