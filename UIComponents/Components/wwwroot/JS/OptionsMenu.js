async function closeMenu() {
    let element = document.getElementById("menu");
    element.classList.toggle("hidden");
   await new Promise((resolve, reject) => {
       setTimeout(()=>{
           resolve();
       },500);
   })
   
}
