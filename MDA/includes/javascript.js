			/*setting cookies*/
			cookie_username = "dataCookie";
			cookie_password = "passwordCookie"
			var YouEnteredUsername;
			var YouEnteredPassword;

			function saveUser() {

			if(document.cookie != document.cookie){ 
				index = document.cookie.indexOf(cookie_username);}
				else 
				{ index = -1;}

			if (index == -1){
				YouEnteredUsername=document.User.username.value;
				document.cookie=cookie_username+"="+YouEnteredUsername+"; expires=Thursday,-Jan-2015 05:00:00 GMT";
				}

			savePassword();
			document.location.href="mainPage.html";
			}
			function savePassword() {

			if(document.cookie != document.cookie){ 
				index = document.cookie.indexOf(cookie_password);}
				else 
				{ index = -1;}

			if (index == -1){
				YouEnteredPassword=document.User.password.value;
				document.cookie=cookie_password+"="+YouEnteredPassword+"; expires=Thursday,-Jan-2015 05:00:00 GMT";
				}

			}

			/*pulling cookies*/

			cookie_name = "dataCookie";
			cookie_pword = "passwordCookie"
			var YouWroteUsername;
			var YouWrotePassword;
			
			function getName() {
			if(document.cookie)
				{
				index = document.cookie.indexOf(cookie_name);
			if (index != -1)
				{
				namestart = (document.cookie.indexOf("=", index) + 1);
				nameend = document.cookie.indexOf(";", index);
			if (nameend == -1) {nameend = document.cookie.length;}
				YouWroteUsername = document.cookie.substring(namestart, nameend);
			return YouWroteUsername;
					}
				}
			}

			YouWroteUsername=getName();
			if (YouWroteUsername == "dataCookie")
				{YouWroteUsername = "Nothing_Entered"}


			function loadName(){
				document.getElementById("displayName").innerHTML="Welcome back "+YouWroteUsername;
				
	
			}
			function getPassword() {
			if(document.cookie)
				{
				index = document.cookie.indexOf(cookie_pword);
			if (index != -1)
				{
				namestart = (document.cookie.indexOf("=", index) + 1);
				nameend = document.cookie.indexOf(";", index);
			if (nameend == -1) {nameend = document.cookie.length;}
				YouWrotePassword = document.cookie.substring(namestart, nameend);
			return YouWrotePassword;
					}
				}
			}

			YouWrotePassworde=getPassword();
			if (YouWrotePassword == "dataCookie")
				{YouWrotePassword = "Nothing_Entered"}


			function loadPassword(){
				document.getElementById("testpassword").innerHTML="Your Password is "+YouWrotePassword;
	
			}

			function loadUserProfile(){
				document.getElementById("currentPassword").value=YouWrotePassword;
				document.getElementById("email").value=YouWroteUsername;

			}