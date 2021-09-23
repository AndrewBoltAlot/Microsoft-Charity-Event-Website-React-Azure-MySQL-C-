import React, { useState, useEffect, useContext } from "react";
import "./ResetPassword.css";
import { Redirect, useHistory } from "react-router-dom";
import { UserContext } from "../../UserContext";
import Popup from '../../components/popup/popup';
import PopupAutoClose from '../../components/popup/popupAutoClose';

var redirectToLogin = false;

const ResetPassword = (props) =>
{
  const [Password, setPassword] = useState("");
  const [NewPassword, setNewPassword] = useState("");
  const [redirect, setRedirect] = useState(false);
  const history = useHistory();
  const [vcode, setVcode] = useState("");
  var Email = getQueryVariable('email');
  const { url, name } = useContext(UserContext);
  var VerificationCode = getQueryVariable('verificationCode');
  const [passwordResetSent, setpasswordResetSent] = useState(false);




  const submit = (e) =>
  {

    e.preventDefault();



    const passwordDetails = {
      Email,
      NewPassword,
      VerificationCode
    };
    if (Password === NewPassword)
    {
      const response = fetch("" + url + "/api/resetForgotPassword", {
        method: "POST",
        headers: { "Content-Type": "application/json; odata=verbose" },
        Accept: 'application/json',
        body: JSON.stringify(passwordDetails)

      }).then(async response =>
      {
        if (!response.ok)
        {
          document.getElementById('errfn').innerHTML = "There was a problem, try opening your link again";
          setTimeout(function () { clearText(); }, 5000);
        }
        else
        {
          setpasswordResetSent(true);
          redirectToLogin = true;

        }
      }).catch(error =>
      {
        console.error("Error :", error);
      })
    }
    else
    {
      document.getElementById('errfn').innerHTML = "Passwords do not match!";
      setTimeout(function () { clearText(); }, 5000);
    }
  }



  function clearText()
  {
    document.getElementById('errfn').innerHTML = " ";
  }



  function getQueryVariable(variable)
  {
    var query = window.location.search.substring(1);

    var vars = query.split("&");

    for (var i = 0; i < vars.length; i++)
    {
      var pair = vars[i].split("=");

      if (pair[0] == variable) { return pair[1]; }
    }
    return (false);
  }

  if (name != "" || VerificationCode.length === undefined || VerificationCode.length < 20)
  {
    return <Redirect to="/Home" />;
  }

  if (redirectToLogin === true)
  {
    return <Redirect to="/Login" />;
  }


  return (
    <div>
      <body class="lobody">
        <div class="Logincontainer2" id="Logincontainer">
          <div class="form-container sign-in-container2">
            <form class="form" onSubmit={submit} onsubmit="return false">
              <h1 class="h1">Reset Password</h1>


              <input
                class="input"
                type="password"
                placeholder="Enter new password"
                required
                pattern="^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$"
                title="Minimum eight characters, at least one letter, one number and one special character."
                onChange={(e) => setPassword(e.target.value)}
              />
              <input
                class="input"
                type="password"
                placeholder="Re-enter new password"
                required
                pattern="^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$"
                title="Minimum eight characters, at least one letter, one number and one special character."
                onChange={(e) => setNewPassword(e.target.value)}
              />

              <button class="button" type="submit">Reset password</button>
              <div>
                <div id="errfn" class="errfn">   </div>
              </div>
            </form>
            {passwordResetSent &&
              <PopupAutoClose closePopup={setpasswordResetSent}
                content={<>
                  <a>Successfully reset password.</a>
                </>}
              />}
          </div>
        </div>
      </body>
    </div>
  );
};

export default ResetPassword;
