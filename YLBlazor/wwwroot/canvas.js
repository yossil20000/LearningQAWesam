// This file is to show how a library package may provide JavaScript interop features
// wrapped in a .NET API
var w, h;
var img;
var canvas, ctx, BRectLeft, BRectTop,  canvasId;
window.addEventListener('resize', windowResize);
window.onscroll = function (e) { windowResize(); }
export function showPrompt(message) {
    return prompt(message, 'Type anything here');
}
function windowResize() {
    canvas.width = window.innerWidth;
    canvas.height = window.innerHeight;
    GetCanvasBoundingRect();
/* erase(false);*/
    scaleToFit(img);
};
function GetCanvasBoundingRect() {
    BRectLeft = document.getElementById(canvasId).getBoundingClientRect().left;
    BRectTop = document.getElementById(canvasId).getBoundingClientRect().top;
};


export function init(id, imageId) {
    canvasId = id;
    canvas = document.getElementById(id);
    ctx = canvas.getContext("2d");
    canvas.width = window.innerWidth;
    canvas.height = window.innerHeight;
    
    w = canvas.width;
    h = canvas.height;
    img = document.getElementById(imageId);
    scaleToFit(img);
    /*ctx.drawImage(img, 0, 0, w, h, 0, 0, w, h);*/
    GetCanvasBoundingRect();
    return "";
    //canvas.addEventListener("mousemove", function (e) {
    //    findxy('move', e)
    //}, false);
    //canvas.addEventListener("mousedown", function (e) {
    //    findxy('down', e)
    //}, false);
    //canvas.addEventListener("mouseup", function (e) {
    //    findxy('up', e)
    //}, false);
    //canvas.addEventListener("mouseout", function (e) {
    //    findxy('out', e)
    //}, false);
}

export function color(obj) {
    switch (obj.id) {
        case "green":
            x = "green";
            break;
        case "blue":
            x = "blue";
            break;
        case "red":
            x = "red";
            break;
        case "yellow":
            x = "yellow";
            break;
        case "orange":
            x = "orange";
            break;
        case "black":
            x = "black";
            break;
        case "white":
            x = "white";
            break;
    }
    if (x == "white") y = 14;
    else y = 2;

}

export function draw(prevX, prevY, currX, currY) {
    
    ctx.beginPath();
    //ctx.moveTo(prevX - canvas.offsetLeft, prevY - canvas.offsetTop);
    //ctx.lineTo(currX - canvas.offsetLeft, currY - canvas.offsetTop);
    //ctx.moveTo(prevX - canvas.offsetLeft - BRectLeft, prevY - canvas.offsetTop - BRectTop);
    //ctx.lineTo(currX - canvas.offsetLeft - BRectLeft, currY - canvas.offsetTop - BRectTop);
    var scaleX, scaleY, rect;
    rect = document.getElementById(canvasId).getBoundingClientRect();

    scaleX = canvas.width / rect.width;
    scaleY = canvas.height / rect.height;

    ctx.moveTo((prevX - BRectLeft) * scaleX, (prevY - BRectTop) * scaleY);
    ctx.lineTo((currX - BRectLeft) * scaleX, (currY - BRectTop) * scaleY);
    //ctx.moveTo(prevX - BRectLeft, prevY - BRectTop);
    //ctx.lineTo(currX - BRectLeft, currY - BRectTop);
    ctx.strokeStyle = "red";
    ctx.lineWidth = 2;
    ctx.stroke();
    ctx.closePath();

    return "";
}
function scaleToFit(img) {
    // get the scale
    var scale = Math.min(canvas.width / img.width, canvas.height / img.height);
    // get the top left position of the image
    var x = (canvas.width / 2) - (img.width / 2) * scale;
    var y = (canvas.height / 2) - (img.height / 2) * scale;
    ctx.drawImage(img, x, y, img.width * scale, img.height * scale);
}
export function erase(isconfirm) {
    var m = true;
    if (isconfirm) {
        m = confirm("Want to clear");

    }
    if (m) {
        clearCanvas();
    }
    return "Cleard";
}
function clearCanvas() {
    ctx.clearRect(0, 0, w, h);
    /*document.getElementById("canvasimg").style.display = "none";*/
    scaleToFit(img);
}
export function newline() {
    first = true;
}

export function save() {
    document.getElementById("canvasimg").style.border = "2px solid";
    var dataURL = canvas.toDataURL();
    document.getElementById("canvasimg").src = dataURL;
    document.getElementById("canvasimg").style.display = "inline";
}

export function findxy(res, e) {
    if (res == 'down') {
        prevX = currX;
        prevY = currY;
        currX = e.clientX - canvas.offsetLeft;
        currY = e.clientY - canvas.offsetTop;

        flag = true;
        dot_flag = true;
        if (dot_flag) {
            ctx.beginPath();
            ctx.fillStyle = x;
            ctx.fillRect(currX, currY, 2, 2);
            ctx.closePath();
            dot_flag = false;
        }
    }
    if (res == 'up' || res == "out") {
        flag = false;


    }
    if (res == 'up') {

        if (first) {
            first = false;
        }
        else {
            draw();
        }

    }
    if (res == 'move') {
        if (flag) {
            prevX = currX;
            prevY = currY;
            currX = e.clientX - canvas.offsetLeft;
            currY = e.clientY - canvas.offsetTop;
            draw();
        }
    }
}

//function refresh(referenceWidth, referenceHeight, drawFunction) {
//    const myCanvas = document.getElementById("myCanvas");
//    myCanvas.width = myCanvas.clientWidth;
//    myCanvas.height = myCanvas.clientHeight;

//    const ratio = Math.min(
//        myCanvas.width / referenceWidth,
//        myCanvas.height / referenceHeight
//    );
//    const ctx = myCanvas.getContext("2d");
//    ctx.scale(ratio, ratio);

//    drawFunction(ctx, ratio);
//    window.requestAnimationFrame(() => {
//        refresh(referenceWidth, referenceHeight, drawFunction);
//    });
//}

//refresh(100, 100, (ctx, ratio) => {
//    const referenceLineWidth = 1;
//    ctx.lineWidth = referenceLineWidth / ratio;
//    ctx.beginPath();
//    ctx.strokeStyle = "blue";
//    ctx.arc(50, 50, 49, 0, 2 * Math.PI);
//    ctx.stroke();

//    ctx.beginPath();
//    ctx.strokeStyle = "blue";
//    ctx.rect(0, 0, 100, 100);
//    ctx.stroke();
//});