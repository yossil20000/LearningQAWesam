// This file is to show how a library package may provide JavaScript interop features
// wrapped in a .NET API

let canvasClasses = new Map();
function addGlobalEventListener(type, selector, callback) {
    document.addEventListener(type, e => {
        if (e.target.matches(selector)) callback(e)
    })
}
function addWindowEventListener(type, callback) {
    window.addEventListener(type, callback);
}
addGlobalEventListener("click", "canvas", e =>
{
    console.log(e.target.id);
}
)
addGlobalEventListener("click", "canvas", e =>
{
    var canvas = canvasClasses.get(e.target.id);
    if (canvas != undefined) {
        var canvas = canvasClasses.get(e.target.id);
        if (canvas === null)
            return ;
        canvas.onMouseDown(e);
    }
    
}
)
addWindowEventListener("resize", CanvasResize);
addWindowEventListener("scroll", CanvasResize);

export function showPrompt(message) {
    return prompt(message, 'Type anything here');
}
export function initCanvas(canvasId, imageId) {
    var canvas = canvasClasses.get(canvasId);
    if (canvas) {
        canvasClasses.delete(canvas);
        canvas = null;
        canvas = undefined;
    }
    if (canvas === undefined) {
        var canvas = canvas = document.getElementById(canvasId);
        if (canvas === null)
            return "canvas === null";
        canvasClasses.set(canvasId, new CanvasClass(canvasId, imageId));
    }
    return "initCanvas";
}


export function OnDrawPreview(x, y, canvasId) {
    var canvas = canvasClasses.get(canvasId);
    if (canvas != undefined) {
        var canvas = canvasClasses.get(canvasId);
        if (canvas === null)
            return "";

    }
    return "OnDrawPreview";
}
export function CanvasResize() {
    /* canvasClasses.forEach(
       (value,key) => { value.onCanvasSizeChange}
    ) */

    for (let value of canvasClasses.values()) {
        value.windowResize();
    }
    //canvasClasses.get('can').onCanvasSizeChange();
    return "CanvasResize";
}
export function CanvasRedraw() {
    /* canvasClasses.forEach(
       (value,key) => { value.onCanvasSizeChange}
    ) */

    for (let value of canvasClasses.values()) {
        value.onCanvasSizeChange();
    }
    //canvasClasses.get('can').onCanvasSizeChange();
    return "CanvasRedraw";
}
export function OnClearDraw(canvasId) {
    var id = canvasClasses.get(canvasId);
    if (id) {
        id.clearDraw();
    }
    return "OnClearDraw";
}
export function OnNewline(canvasId) {
    var id = canvasClasses.get(canvasId);
    if (id) {
        id.SetNewLine();
    }
    
    return "OnNewline";
}
export function OnUnDo(canvasId) {
    var id = canvasClasses.get(canvasId);
    if (id) {
        id.unDo();
    }
    return "OnUnDo";
}
export function mouseDraw(canvasId) {
    var id = canvasClasses.get(canvasId);
    if (id) {
        id.newLine = false;
        id.bDrawOnMove = true;
    }
    return "mouseDraw";
}
export function setCanvasImage(canvasId, imageId) {
    
    var id = canvasClasses.get(canvasId);
    if (id != null) {
        id.setImage(imageId);
    }
    return "setCanvasImage";
}

class CanvasClass {
    canvas;
    img;
    canvasId;
    imageId;
    ctx;
    prevX;
    currX;
    prevY;
    currY;
    bDrawOnMove = false;
    BRectLeft;
    BRectTop;
    scaleMode = 2;
    pathsarray = [];
    points = [];
    newLine = true;
    lastDraw = { x: -1, y: -1 };
    first = true;
    cuurentStrokStyle = 'red';
    constructor(canvasId,imageId) {
        this.canvasId = canvasId;
        this.canvas = document.getElementById(canvasId);
        this.imageId = imageId;
        
        this.ctx = this.canvas.getContext('2d');
        this.ctx.strokeStyle = this.cuurentStrokStyle;
        this.GetCanvasBoundingRect();
        this.canvas.width = window.innerWidth;
        this.canvas.height = window.innerHeight;
        //this.canvas.addEventListener(
        //    'mousemove',
        //    (e) => {
        //        this.move(e);
        //    }
        //);
        //this.canvas.addEventListener(
        //    'mousedown',
        //    (e) => {
        //        this.onMouseDown(e);
        //    }
        //);
        //this.canvas.addEventListener(
        //    'mouseup',
        //    (e) => {
        //        this.bDrawOnMove = false;
        //    },
        //    false
        //);

    }
    RefreshContext() {
        if (this.ctx)
            return;
        this.ctx = this.canvas.getContext('2d');
        this.ctx.strokeStyle = this.cuurentStrokStyle;
    }
    unDo() {
        this.pathsarray.pop();
        this.drawPath();
    }
    onCanvasSizeChange() {

        this.windowResize();
    }
    move(e) {
        if (this.bDrawOnMove) {
            this.prevX = this.currX;
            this.prevY = this.currY;
            this.currX = e.clientX - this.canvas.offsetLeft;
            this.currY = e.clientY - this.canvas.offsetTop;
            this.draw();
        }
    }
    onMouseDown(e) {
        if (this.bDrawOnMove === true) {
        }
        else {
            if (this.newLine === true) {
                this.newLine = false;
                this.prevX = e.clientX;
                this.prevY = e.clientY;
                return;
            }
            this.draw(e.clientX, e.clientY);
        }
    }
    windowResize() {
        this.canvas.width = window.innerWidth;
        this.canvas.height = window.innerHeight;
        this.GetCanvasBoundingRect();
        /* erase(false);*/
        this.scaleToFit(this.img);
        this.drawPath();
    }
    GetCanvasBoundingRect() {
        var canvas = document
            .getElementById(this.canvasId);
        if (canvas) {
            this.BRectLeft = canvas.getBoundingClientRect().left;
            this.BRectTop = canvas.getBoundingClientRect().top;
        }

       
    }
    draw(currX, currY) {
        //this.windowResize();
        var scaleX, scaleY, rect;
        rect = document.getElementById(this.canvasId).getBoundingClientRect();

        scaleX = this.canvas.width / rect.width;
        scaleY = this.canvas.height / rect.height;
        this.ctx.beginPath();

        this.ctx.moveTo((this.prevX - this.BRectLeft) * scaleX, (this.prevY - this.BRectTop) * scaleY);
        this.ctx.lineTo((currX - this.BRectLeft) * scaleX, (currY - this.BRectTop) * scaleY);
        //ctx.moveTo(prevX - BRectLeft, prevY - BRectTop);
        //ctx.lineTo(currX - BRectLeft, currY - BRectTop);
        this.ctx.strokeStyle = this.cuurentStrokStyle;;
        this.ctx.lineWidth = 2;
        this.ctx.stroke();
        this.ctx.closePath();
        this.lastDraw.x = currX;
        this.lastDraw.y = currY;
        this.points = [];
        this.points.push({ x: this.prevX, y: this.prevY });
        this.points.push({ x: currX, y: currY });

        this.pathsarray.push(this.points);
        this.prevX = currX;
        this.prevY = currY;
        return '';
    }

    drawPath() {
        this.clearCanvas();
        var dpoint;
        this.ctx.beginPath();
        this.ctx.strokeStyle = this.cuurentStrokStyle;
        this.pathsarray.forEach((path) => {
            dpoint = this.GetDrawingPoint(path[0]);
            this.ctx.moveTo(dpoint.x, dpoint.y);
            for (let i = 1; i < path.length; i++) {
                dpoint = this.GetDrawingPoint(path[i]);
                this.ctx.lineTo(dpoint.x, dpoint.y);
                this.prevX = path[i].x;
                this.prevY = path[i].y;
            }
            this.ctx.stroke();
        });
        this.ctx.closePath();

    }
    GetDrawingPoint(point) {
        var scaleX, scaleY, rect;
        rect = document.getElementById(this.canvasId).getBoundingClientRect();

        scaleX = this.canvas.width / rect.width;
        scaleY = this.canvas.height / rect.height;
        return {
            x: (point.x - this.BRectLeft) * scaleX, y: (point.y - this.BRectTop) * scaleY
        }
    }

    clearCanvas() {
        this.ctx.clearRect(0, 0, this.canvas.width, this.canvas.height);
        this.scaleToFit(this.img);
        this.SetNewLine();
    }
    clearDraw() {
        this.points = [];
        this.pathsarray = [];
        //newline();
        this.drawPath();
    }
    SetNewLine() {
        this.newLine = true;
        this.bDrawOnMove = false;
        this.lastDraw = { x: -1, y: -1 };
    }
    setImage(imagId) {
        this.img = document.getElementById(imagId);
        if (this.img != null) {
            this.imageId = imagId;
            this.scaleToFit(this.img);
        }


    }
    scaleToFit(img) {
        if (img === undefined)
            return;
        if (img == null)
            return;
        // get the scale
        this.RefreshContext();
        if (this.scaleMode == 0) {
            this.ctx.drawImage(this.img, 0, 0);
        } else if (this.scaleMode == 1) {
            var scaleWidth = this.canvas.width / this.img.width;
            this.ctx.drawImage(
                this.img,
                0,
                0,
                this.img.width * scaleWidth,
                this.img.height * scaleWidth
            );
        } else if ((this.scaleMode = 2)) {
            var scaleWidth = this.canvas.width / this.img.width;
            var scaleHeight = this.canvas.height / this.img.height;
            this.ctx.drawImage(
                this.img,
                0,
                0,
                this.img.width * scaleWidth,
                this.img.height * scaleHeight
            );
        } else {
            var scale = Math.min(
                this.canvas.width / this.img.width,
                this.canvas.height / this.img.height
            );
            // get the top left position of the image
            var x = this.canvas.width / 2 - (this.img.width / 2) * scale;
            var y = this.canvas.height / 2 - (this.img.height / 2) * scale;
            this.ctx.drawImage(this.img, x, y, this.img.width * scale, this.img.height * scale);
        }
    }
}
//function windowResize() {
//    canvas.width = window.innerWidth;
//    canvas.height = window.innerHeight;
//    GetCanvasBoundingRect();
//    /* erase(false);*/
//    scaleToFit(img);
//};
//function GetCanvasBoundingRect() {
//    BRectLeft = document.getElementById(canvasId).getBoundingClientRect().left;
//    BRectTop = document.getElementById(canvasId).getBoundingClientRect().top;
//};
//export function UpdateImage(imageId) {
//    img = document.getElementById(imageId);
//    scaleToFit(img);
//}

//export function init(id, imageId) {

//    canvasId = id;
//    canvas = document.getElementById(id);
//    ctx = canvas.getContext("2d");
//    canvas.width = window.innerWidth;
//    canvas.height = window.innerHeight;

//    w = canvas.width;
//    h = canvas.height;
//    img = document.getElementById(imageId);
//    scaleToFit(img);
//    /*ctx.drawImage(img, 0, 0, w, h, 0, 0, w, h);*/
//    GetCanvasBoundingRect();
//    return "";
//    //canvas.addEventListener("mousemove", function (e) {
//    //    findxy('move', e)
//    //}, false);
//    //canvas.addEventListener("mousedown", function (e) {
//    //    findxy('down', e)
//    //}, false);
//    //canvas.addEventListener("mouseup", function (e) {
//    //    findxy('up', e)
//    //}, false);
//    //canvas.addEventListener("mouseout", function (e) {
//    //    findxy('out', e)
//    //}, false);
//}

//export function color(obj) {
//    switch (obj.id) {
//        case "green":
//            x = "green";
//            break;
//        case "blue":
//            x = "blue";
//            break;
//        case "red":
//            x = "red";
//            break;
//        case "yellow":
//            x = "yellow";
//            break;
//        case "orange":
//            x = "orange";
//            break;
//        case "black":
//            x = "black";
//            break;
//        case "white":
//            x = "white";
//            break;
//    }
//    if (x == "white") y = 14;
//    else y = 2;

//}
//export function drawPreview(x, y) {

//    if (lastDraw.x != -1) {
//        ctx.strokeStyle = "blue";
//        drawPath();
//        ctx.beginPath();
//        var dpoint = GetDrawingPoint(lastDraw);
//        ctx.moveTo(dpoint.x, dpoint.y);
//        dpoint = GetDrawingPoint({ x: x, y: y });
//        ctx.lineTo(dpoint.x, dpoint.y);
//        ctx.stroke();
//        ctx.closePath();
//    }

//}
//function drawPath() {
//    erase(false);
//    var dpoint;
//    ctx.beginPath();
//    pathsarray.forEach(path => {

//        dpoint = GetDrawingPoint(path[0]);
//        ctx.moveTo(dpoint.x, dpoint.y);
//        for (let i = 1; i < path.length; i++) {
//            dpoint = GetDrawingPoint(path[i]);
//            ctx.lineTo(dpoint.x, dpoint.y);
//        }
//        ctx.stroke();
//    })
//    ctx.closePath();
//}

//function GetDrawingPoint(point) {
//    var scaleX, scaleY, rect;
//    rect = document.getElementById(canvasId).getBoundingClientRect();

//    scaleX = canvas.width / rect.width;
//    scaleY = canvas.height / rect.height;
//    return {
//        x: (point.x - BRectLeft) * scaleX, y: (point.y - BRectTop) * scaleY
//    }
//}
//export function draw(prevX, prevY, currX, currY) {

//    ctx.beginPath();
//    //ctx.moveTo(prevX - canvas.offsetLeft, prevY - canvas.offsetTop);
//    //ctx.lineTo(currX - canvas.offsetLeft, currY - canvas.offsetTop);
//    //ctx.moveTo(prevX - canvas.offsetLeft - BRectLeft, prevY - canvas.offsetTop - BRectTop);
//    //ctx.lineTo(currX - canvas.offsetLeft - BRectLeft, currY - canvas.offsetTop - BRectTop);
//    var scaleX, scaleY, rect;
//    rect = document.getElementById(canvasId).getBoundingClientRect();

//    scaleX = canvas.width / rect.width;
//    scaleY = canvas.height / rect.height;

//    ctx.moveTo((prevX - BRectLeft) * scaleX, (prevY - BRectTop) * scaleY);
//    ctx.lineTo((currX - BRectLeft) * scaleX, (currY - BRectTop) * scaleY);
//    //ctx.moveTo(prevX - BRectLeft, prevY - BRectTop);
//    //ctx.lineTo(currX - BRectLeft, currY - BRectTop);
//    ctx.strokeStyle = "red";
//    ctx.lineWidth = 2;
//    ctx.stroke();
//    ctx.closePath();
//    lastDraw.x = currX;
//    lastDraw.y = currY;
//    points = [];
//    points.push({ x: prevX, y: prevY });
//    points.push({ x: currX, y: currY });
//    pathsarray.push(points);
//    return "";
//}
//function scaleToFit(img) {
//    // get the scale
//    if (scaleMode == 0) {
//        ctx.drawImage(img, 0, 0);
//    }
//    else if (scaleMode == 1) {
//        var scaleWidth = canvas.width / img.width;
//        ctx.drawImage(img, 0, 0, img.width * scaleWidth, img.height * scaleWidth);
//    }
//    else if (scaleMode = 2) {
//        var scaleWidth = canvas.width / img.width;
//        var scaleHeight = canvas.height / img.height;
//        ctx.drawImage(img, 0, 0, img.width * scaleWidth, img.height * scaleHeight);
//    }
//    else {
//        var scale = Math.min(canvas.width / img.width, canvas.height / img.height);
//        // get the top left position of the image
//        var x = (canvas.width / 2) - (img.width / 2) * scale;
//        var y = (canvas.height / 2) - (img.height / 2) * scale;
//        ctx.drawImage(img, x, y, img.width * scale, img.height * scale);
//    }


//}
//export function erase(isconfirm) {
//    var m = true;
//    if (isconfirm) {
//        m = confirm("Want to clear");

//    }
//    if (m) {
//        clearCanvas();
//    }
//    return "Cleard";
//}
//function clearCanvas() {
//    ctx.clearRect(0, 0, w, h);


//    /*document.getElementById("canvasimg").style.display = "none";*/
//    scaleToFit(img);
//}
//export function ClearDraw() {
//    points = [];
//    pathsarray = [];
//    newline();
//    drawPath();
//}
//export function newline() {

//    lastDraw = { x: -1, y: -1 };
//}

//export function save() {
//    document.getElementById("canvasimg").style.border = "2px solid";
//    var dataURL = canvas.toDataURL();
//    document.getElementById("canvasimg").src = dataURL;
//    document.getElementById("canvasimg").style.display = "inline";
//}

//export function findxy(res, e) {
//    if (res == 'down') {
//        prevX = currX;
//        prevY = currY;
//        currX = e.clientX - canvas.offsetLeft;
//        currY = e.clientY - canvas.offsetTop;

//        flag = true;
//        dot_flag = true;
//        if (dot_flag) {
//            ctx.beginPath();
//            ctx.fillStyle = x;
//            ctx.fillRect(currX, currY, 2, 2);
//            ctx.closePath();
//            dot_flag = false;
//        }
//    }
//    if (res == 'up' || res == "out") {
//        flag = false;


//    }
//    if (res == 'up') {

//        if (first) {
//            first = false;
//        }
//        else {
//            draw();
//        }

//    }
//    if (res == 'move') {
//        if (flag) {
//            prevX = currX;
//            prevY = currY;
//            currX = e.clientX - canvas.offsetLeft;
//            currY = e.clientY - canvas.offsetTop;
//            draw();
//        }
//    }
//}

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