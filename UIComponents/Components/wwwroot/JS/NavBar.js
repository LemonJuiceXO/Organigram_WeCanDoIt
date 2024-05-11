var tabs=document.querySelectorAll(".navTab");
var savedSelectedTab;

tabs.forEach(value => {
    
    value.onmouseenter=()=>{
        tabs.forEach(value1 => {
            if(value1!=value){
                if(value1.classList.contains("selectedTab")){
                    value1.classList.toggle("anotherTabIsHovered");
                    savedSelectedTab=value1;
                }
            }
        })
    }
    
    value.onmouseleave=()=>{
        if(savedSelectedTab!=null){
            if(savedSelectedTab.classList.contains("anotherTabIsHovered")){
                savedSelectedTab.classList.toggle("anotherTabIsHovered");
            }
        }
    }
    value.onclick=()=>{
        savedSelectedTab=null;
    }
    
})
function selectTab(selectedTab){
    selectedTab.classList.add("selectedTab");
    
    tabs.forEach(tab => {
        if(tab!=selectedTab){
            tab.classList.remove("selectedTab");
        }
    })
    
}

window.addEventListener("scroll",()=>{
   
   let scrollY= window.scrollY;
   let navBar=document.getElementById("navBar");
   
   if(scrollY>200 && navBar.classList.contains("navStatic")){
       navBar.classList.remove("navStatic");
       navBar.classList.add("navScrolled");
   }
   else if(scrollY<120){
       navBar.classList.add("navStatic");
       navBar.classList.remove("navScrolled");
   }
})

