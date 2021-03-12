
export function initCanvas(canvasId) {
   
    return "initCanvas";
}
export function OnDrawPreview(int x, int y) {
    
    return "OnDrawPreview";
}

export function CanvasRedraw() {
    
    return "CanvasRedraw";
}
export function OnClearDraw(canvasId) {
   
    return "OnClearDraw";
}
export function OnNewline(canvasId) {
    
    return "OnNewline";
}
export function OnUnDo(canvasId) {
   
    return "OnUnDo";
}
export function mouseDraw(canvasId) {
    
    return "mouseDraw";
}
export function setCanvasImage(canvasId, imageId) {
    
    return "setCanvasImage";
}

export function showPrompt(message) {
    return prompt(message, 'Type anything here');
}