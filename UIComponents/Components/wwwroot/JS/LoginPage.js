   var showPasswordIcon= document.getElementById("showPassword");
   showPasswordIcon.addEventListener("mouseenter",ShowHidePassword);
   showPasswordIcon.addEventListener("mouseleave",ShowHidePassword);   
    function ShowHidePassword(){
       var passwordField=document.getElementById("LoginPasswordField")
        passwordField.type=(passwordField.type==="password")?"text":"password";
       var passwordIcon=document.getElementById("showPassword");
       if(passwordIcon.innerHTML.includes("M9.60997")){
           passwordIcon.innerHTML="<path d=\"M3 14C3 9.02944 7.02944 5 12 5C16.9706 5 21 9.02944 21 14M17 14C17 16.7614 14.7614 19 12 19C9.23858 19 7 16.7614 7 14C7 11.2386 9.23858 9 12 9C14.7614 9 17 11.2386 17 14Z\" stroke=\"#000000\" stroke-width=\"2\" stroke-linecap=\"round\" stroke-linejoin=\"round\"/>\n";
       }
       else{
           passwordIcon.innerHTML="<path d=\"M9.60997 9.60714C8.05503 10.4549 7 12.1043 7 14C7 16.7614 9.23858 19 12 19C13.8966 19 15.5466 17.944 16.3941 16.3878M21 14C21 9.02944 16.9706 5 12 5C11.5582 5 11.1238 5.03184 10.699 5.09334M3 14C3 11.0069 4.46104 8.35513 6.70883 6.71886M3 3L21 21\" stroke=\"#000000\" stroke-width=\"2\" stroke-linecap=\"round\" stroke-linejoin=\"round\"/>\n";
       }
    }