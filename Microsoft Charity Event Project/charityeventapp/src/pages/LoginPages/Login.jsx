import React, { useState, useEffect, useContext } from "react";
import "./login.css";
import { Redirect, useHistory } from "react-router-dom";
import { UserContext } from "../../UserContext";
import Popup from '../../components/popup/popup';
import PopupAutoClose from '../../components/popup/popupAutoClose';


const Login = (props) =>
{
  const [Email, setEmail] = useState("");
  const [resetEmail, setresetEmail] = useState("");
  const [Password, setPassword] = useState("");
  const [Type, setType] = useState("Participant");
  const [redirect, setRedirect] = useState(false);
  const history = useHistory();
  const { user5, setUser5, name, setName, url } = useContext(UserContext);
  const [vcode, setVcode] = useState("");
  const [isVerify, setIsVerify] = useState(false);
  const [forgotPasswordPressed, setforgotPasswordPressed] = useState(false);
  const [passwordResetSent, setpasswordResetSent] = useState(false);

  const login = (e) =>
  {
    e.preventDefault();

    //setUser5(Type);
    if (name === "")
    {
      fetch("" + url + "/api/login", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        //credentials: "include",
        body: JSON.stringify({
          Email,
          Password,
        }),
      })
        .then((res) =>
        {
          if (!res.ok)
          {
            document.getElementById('errfn').innerHTML = "Incorrect Email or Password";
            setTimeout(function () { clearText(); }, 5000);
          } else
          {
            setIsVerify(true);

          }
        })
        .catch((err) =>
        {

        });
    }
  };

  const resetPassword = (e) =>
  {

    if (validateEmail(resetEmail))
    {

      setpasswordResetSent(true);
      e.preventDefault();

      //setUser5(Type);
      if (name === "")
      {
        fetch("" + url + "/api/sendResetPasswordLink/" + resetEmail + "", {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          credentials: "include",
        }).then(res =>  
        {
          if (!res.ok)
          {

          } else
          {
            //create pop up saying
            window.location.reload();
          }
        }).catch(err =>
        {

        });
      }
    } else
    {
      document.getElementById('errfn2').innerHTML = "Email is not in the correct format.";
      setTimeout(function () { clearText(); }, 5000);
    }
  };


  function validateEmail(email) 
  {
    var re = /\S+@\S+\.\S+/;
    return re.test(email);
  }

  const TFacV = (e) =>
  {
    e.preventDefault();

    setUser5(Type);

    fetch("" + url + "/api/verify2FA", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
      body: JSON.stringify({
        Email,
        "VerificationCode": vcode,
      }),
    })
      .then((res) =>
      {
        if (!res.ok)
        {
          alert("Incorrect Code");
        } else
        {
          setRedirect(true);
          window.location.reload();
        }
      })
      .catch((err) =>
      {

      });

  }

  function clearText()
  {
    document.getElementById('errfn').innerHTML = " ";
  }

  if (redirect)
  {
    return <Redirect to="/Home" />;
  }

  if (name != "")
  {
    return <Redirect to="/Home" />;
  }

  const changeAccount = () =>
  {
    if (Type === "Participant")
    {
      setType("Organiser")
    } else
    {
      setType("Participant")
    }

  }

  return (
    <div>
      <body class="lobody">
        <div class="Logincontainer" id="Logincontainer">
          <div class="form-container sign-in-container">
            <form class="form" onSubmit={login}>
              <h1 class="h1">Sign In</h1>
              <p></p>
              <span class="span">Choose your account type to login</span>

              <div id="addmorespace"> &nbsp;
                <div class="addSpace2"> Participant  &nbsp; <label class="switch">
                  <input type="checkbox" id="accountTypeCB" onChange={(e) => changeAccount()} />
                  <span class="slider round"></span>
                </label>  &nbsp; Organiser </div>
              </div>

              <input
                class="input"
                type="email"
                placeholder="Email"
                onChange={(e) => setEmail(e.target.value.toLowerCase())}

              />
              <input
                class="input"
                type="password"
                placeholder="Password"
                required
                pattern="^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$"
                title="Minimum eight characters, at least one letter, one number and one special character."
                onChange={(e) => setPassword(e.target.value)}
              />
              <a class="a" href="#" onClick={() => setforgotPasswordPressed(true)}>
                Forgot your password?
              </a>
              <button class="button" type="submit">
                Sign In
              </button>
              <div>
                <div id="errfn" class="errfn">   </div>
              </div>
            </form>

          </div>

          <div class="overlay-container">
            <div class="overlay">
              <div class="overlay-panel overlay-left">
                <h1>Welcome Back!</h1>
                <p>
                  To keep connected with us please login with your personal info
                </p>
                <button class="ghost" id="signIn">
                  Sign In
                </button>
              </div>
              <div class="overlay-panel overlay-right">
                <h1 class="h1">Hello, Friend!</h1>
                <p class="p">
                  Enter your personal details and start journey with us
                </p>
                <button
                  class="button ghost"
                  id="signUp"
                  onClick={() =>
                  {
                    history.push("/SignUp");
                  }}
                >
                  Sign Up
                </button>
              </div>
            </div>

            {isVerify &&
              <Popup closePopup={setIsVerify}
                content={<>
                  <input type="text" class="form-control" placeholder="Enter your verification code" onChange={(e) => setVcode(e.target.value)} />
                  <button type="button" class="button" onClick={TFacV}>Verify</button>
                </>}
              />}
            {forgotPasswordPressed &&
              <Popup closePopup={setforgotPasswordPressed}
                content={<>
                  <input type="email" class="form-control" placeholder="Enter your Email"
                    required
                    onChange={(e) => setresetEmail(e.target.value)} />
                  <button type="button" class="button" onClick={resetPassword}>Send Password reset</button>
                  <div>
                    <div id="errfn2" class="errfn">   </div>
                  </div>

                </>}
              />}


            {passwordResetSent &&
              <PopupAutoClose closePopup={setpasswordResetSent}
                content={<>
                  <a>A link to reset your password has been sent to {resetEmail}.</a>
                </>}
              />}
          </div>

        </div>
      </body>
    </div>
  );
};

export default Login;
