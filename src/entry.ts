const GRAVITY = 9.2;
const GAME_SPEED_X = 100;
const canvasWidthHeight = Math.min(Math.min(window.innerHeight, window.innerWidth), 512);
const TUBE_POS_LIST: number[] = [
    canvasWidthHeight + 50,
    canvasWidthHeight + 250,
    canvasWidthHeight + 480
];


stage.interactive = true;
stage.hitArea = new PIXI.Rectangle(0, 0, 1000, 1000);
renderer.render(stage);

const tubeList = TUBE_POS_LIST.map(d => new Tube(stage, d));

let gameStarted = false;
let gameFailed = false;

button.addEventListener('click', () => {
    gameStarted = true;
    button.innerHTML = 'Retry';
    if (gameFailed) {
        gameFailed = false;
        tubeList.forEach((d, i) => d.reset(TUBE_POS_LIST[i]));
        bird.reset();
    }
    button.classList.add('hide');
});
