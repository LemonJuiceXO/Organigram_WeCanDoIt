

window.onmousedown=ev => {
    baseX=ev.clientX;
    baseY=ev.clientY;
}

function Clickme(element){
    
    element.onmousemove=eve=>{
        drag(element,eve);
    };
    
    element.onmouseup=ev=>{
        baseX=ev.clientX;
        baseY=ev.clientY;
        
        element.onmousemove=null;
        element.onmouseup=null;
    }
}

function drag(element,ev){
    ev.preventDefault();
    
    const finalY=(baseY-ev.clientY);
    const finalX=(baseX-ev.clientX);
    baseX=ev.clientX;
    baseY=ev.clientY;
    element.style.top=(element.offsetTop-finalY)+"px";
    element.style.left=(element.offsetLeft-finalX)+"px";
}

function linkNodes(parentNodeId, subNodeId) {
    let Parent = document.getElementById(parentNodeId);
    let Child = document.getElementById(subNodeId);
    let container = document.getElementById("listNodes");
    let svg = document.createElementNS("http://www.w3.org/2000/svg", "svg");
    let polyline = document.createElementNS("http://www.w3.org/2000/svg", "polyline");
    svg.setAttribute("style","position:absolute; pointer-events: none;  ")
    svg.setAttribute("width","100%");
    svg.setAttribute("height","100%");
    polyline.setAttribute("stroke","2px");
    polyline.setAttribute("style","z-index:1; stroke-width: 2px; stroke: black !important; width:100%; height:100%;");
    
    function updateLinePosition() {
        let Width=Math.abs((Parent.offsetLeft + Parent.offsetWidth / 2)-(Child.offsetLeft + Child.offsetWidth / 2) );
        let Height=Math.abs((Parent.offsetTop + Parent.offsetHeight / 2)-(Child.offsetTop + Child.offsetHeight / 2));
        
        let points = "" + (Parent.offsetLeft + Parent.offsetWidth / 2) + "," + (Parent.offsetTop + Parent.offsetHeight / 2) + " " + (Child.offsetLeft + Child.offsetWidth / 2) + "," + (Child.offsetTop + Child.offsetHeight / 2);
        polyline.setAttribute("points", points);
        svg.setAttribute("width",container.scrollWidth);
        svg.setAttribute("height",container.scrollHeight);
    }   
    

    updateLinePosition();
    
    svg.appendChild(polyline);
    container.appendChild(svg);

    const config = {
        attributes: true,
    };

    const callback = function(mutationsList, observer) {
        for (let mutation of mutationsList) {
   
                updateLinePosition();
        
        }
    };
    
    const observer = new MutationObserver(callback);

    observer.observe(Parent, config);
    observer.observe(Child, config);
}








