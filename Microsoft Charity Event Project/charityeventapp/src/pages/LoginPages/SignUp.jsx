import "./Signup.css";
import { Redirect } from "react-router-dom";
import { useState, useContext } from "react";
import moment from 'moment';
import { UserContext } from "../../UserContext";

const SignUp = () =>
{
  const [Email, setEmail] = useState("");
  const [Password, setPassword] = useState("");
  const [repeatPassword, setRepeatPassword] = useState("");
  const [FirstName, setFirstName] = useState("");
  const [LastName, setLastName] = useState("");
  const [PhoneNumber, setPhoneNumber] = useState("");
  const [DOB, setDOB] = useState();
  const [Address, setAddress] = useState("");
  const [Zip, setZip] = useState("");
  const [CompanyName, setCompanyName] = useState("");
  const [Description, setDescription] = useState("");
  const [Type, setType] = useState("User");
  const [redirect, setRedirect] = useState(false);
  let menu;
  const { url, name } = useContext(UserContext);


  const clearUser = () =>
  {
    document.getElementById("u1").value = "";
    document.getElementById("u2").value = "";
    document.getElementById("u3").value = "";
    document.getElementById("u4").value = "";
    document.getElementById("u5").value = "";
    document.getElementById("u6").value = "";
    document.getElementById("u7").value = "";
    document.getElementById("u8").value = "";
  }

  const clearOrganiser = () =>
  {
    document.getElementById("o1").value = "";
    document.getElementById("o2").value = "";
    document.getElementById("o3").value = "";
    document.getElementById("o4").value = "";
    document.getElementById("o5").value = "";
    document.getElementById("o6").value = "";
    document.getElementById("o7").value = "";
    document.getElementById("o8").value = "";
  }



  const changeAccount = () =>
  {
    if (Type === "User")
    {
      clearUser()
      setType("Organiser")
    } else
    {

      clearOrganiser()
      setType("User")
    }

  }


  const submit = (e) =>
  {
    e.preventDefault();


    if (Password === repeatPassword)
    {
      if (Type === "User")
      {
        const user = {
          Email,
          Password,
          FirstName,
          LastName,
          PhoneNumber,
          DOB,
          Address,
          Zip,
        };

        const respnose = fetch("" + url + "/api/ParticipantSignup", {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify(user),
        })
          .then(async (response) =>
          {
            if (!response.ok)
            {
              document.getElementById('errfn').innerHTML = "The email or phone number is already registered to a " + Type + " account!";
              setTimeout(function () { clearText(); }, 5000);

            } else
            {
              setRedirect(true);

            }
          })
          .catch((error) =>
          {
            console.error("Error :", error);
          });
      } else
      {
        const organiser = {
          Email,
          Password,
          CompanyName,
          Description,
          PhoneNumber,
          Address,
          Zip,
        };

        const respnose = fetch("" + url + "/api/OrganiserSignup", {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify(organiser),
        })
          .then(async (response) =>
          {
            if (!response.ok)
            {
              document.getElementById('errfn').innerHTML = "The email or phone number is already registered to a " + Type + " account!";
              setTimeout(function () { clearText(); }, 5000);

            } else
            {
              setRedirect(true);



            }
          })
          .catch((error) =>
          {
            console.error("Error :", error);
          });
      }
    } else
    {
      document.getElementById('errfn').innerHTML = "Passwords do not match!";
      setTimeout(function () { clearText(); }, 5000);
    }
  };

  if (redirect)
  {
    return <Redirect to="/Login" />;
  }

  if (name != "")
  {
    return <Redirect to="/Home" />;
  }

  function clearText()
  {
    document.getElementById('errfn').innerHTML = " ";
  }

  if (Type === "Organiser")
  {
    menu = (
      <div>
        <input
          id="o1"
          class="input"
          type="text"
          placeholder="Company Name"
          required
          onChange={(e) => setCompanyName(e.target.value)}
          pattern="^[A-Za-z0-9_ ]{5,30}$"
          title="Company name can only contain Alpha-numeric characters: A-Z, a-z, 0-9 & Must be between 5-30 characters."

        />
        <input
          id="o2"
          class="input"
          type="text"
          placeholder="Company Description"
          required
          onChange={(e) => setDescription(e.target.value)}
          pattern="^.{10,150}$"
          title="Description must be between 10 and 300 Characters long."
        />
        <input
          id="o3"
          class="input"
          type="text"
          placeholder="Contact Number"
          required
          onChange={(e) => setPhoneNumber(e.target.value)}
          pattern="^\+(353)(\s*\d){9,12}$"
          title="Not a valid Irish mobile number number. (+353 ** *** ****)"
        />
        <input
          id="o4"
          class="input"
          type="text"
          placeholder="Address"
          required
          onChange={(e) => setAddress(e.target.value)}
          pattern="[A-Za-z0-9'\.\-\s\,]"
          title="Not a valid address."
        />
        <input
          id="o5"
          class="input"
          type="text"
          placeholder="Eircode"
          required
          onChange={(e) => setZip(e.target.value)}
          pattern="(?:^[AC-FHKNPRTV-Y][0-9]{2}|D6W)[ -]?[0-9AC-FHKNPRTV-Y]{4}$"
          title="Not a valid Irish Eircode."
        />
        <input
          id="o6"
          class="input"
          type="email"
          placeholder="Email"
          required
          onChange={(e) => setEmail(e.target.value.toLowerCase())}
          title="Not a valid email address."
        />
        <input
          id="o7"
          class="input"
          type="password"
          placeholder="Password"
          required
          onChange={(e) => setPassword(e.target.value)}
          pattern="^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$"
          title="Minimum eight characters, at least one letter, one number and one special character."
        />
        <input
          id="o8"
          class="input"
          type="password"
          placeholder="Rewrite Password"
          required
          onChange={(e) => setRepeatPassword(e.target.value)}
          pattern="^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$"
          title="Minimum eight characters, at least one letter, one number and one special character."
        />

        <div
          id="u9"
          class="inputSignupspace"



        />

      </div>
    );
  } else
  {
    menu = (
      <div>
        <input
          id="u1"
          class="input"
          type="text"
          placeholder="First Name"
          required
          onChange={(e) => setFirstName(e.target.value)}
          pattern="([a-zA-Z]{3,30}\s*)+"
          title="Not a valid First name."
        />
        <input
          id="u2"
          class="input"
          type="text"
          placeholder="Last Name"
          required
          onChange={(e) => setLastName(e.target.value)}
          pattern="[a-zA-Z]{3,30}"
          title="Not a valid Last name."
        />
        <input
          id="u3"
          class="input"
          type="text"
          placeholder="Contact Number"
          required
          onChange={(e) => setPhoneNumber(e.target.value)}
          pattern="^\+(353)(\s*\d){9,12}$"
          title="Not a valid Irish mobile number number. (+353 ** *** ****)"
        />
        <input
          id="u4"
          class="input"
          type="Date"
          placeholder="Date Of Birth"
          required
          onChange={(e) => setDOB(e.target.value)}
          min={moment().add(-100, 'years').format("YYYY-MM-DD")}
          max={moment().add(-18, 'years').format("YYYY-MM-DD")}
        />
        <input
          id="u5"
          class="input"
          type="text"
          placeholder="Address"
          required
          onChange={(e) => setAddress(e.target.value)}
          pattern="[A-Za-z0-9'\.\-\s\,]"
          title="Not a valid address."
        />
        <input
          id="u6"
          class="input"
          type="text"
          placeholder="Zip"
          required
          onChange={(e) => setZip(e.target.value)}
          pattern="(?:^[AC-FHKNPRTV-Y][0-9]{2}|D6W)[ -]?[0-9AC-FHKNPRTV-Y]{4}$"
          title="Not a valid Irish Eircode."
        />
        <input
          id="u7"
          class="input"
          type="email"
          placeholder="Email"
          required
          onChange={(e) => setEmail(e.target.value.toLowerCase())}
          title="Not a valid email address."
        />
        <input
          id="u8"
          class="input"
          type="password"
          placeholder="Password"
          required
          onChange={(e) => setPassword(e.target.value)}
          pattern="^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$"
          title="Minimum eight characters, at least one letter, one number and one special character."
        />
        <input
          id="u9"
          class="input"
          type="password"
          placeholder="Rewrite Password"
          required
          onChange={(e) => setRepeatPassword(e.target.value)}
          pattern="^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$"
          title="Minimum eight characters, at least one letter, one number and one special character."
        />


      </div>
    );
  }





  return (
    <div class="lobody">
      <div class="Signupcontainer" id="Signupcontainer">
        <form class="form" onSubmit={submit}>
          <h1 class="h1">Create Account</h1>
          <span class="span">Choose user or organiser account type</span>

          <div id="addmorespace"> &nbsp;
            <div class="addSpace"> Participant  &nbsp; <label class="switch">
              <input type="checkbox" id="accountTypeCB" onChange={(e) => changeAccount()} />
              <span class="slider round"></span>
            </label>  &nbsp; Organiser </div>
          </div>

          {menu}
          <button class="button" type="submit">
            SignUp
          </button>

          <div>
            <div id="errfn" class="errfn">   </div>
          </div>

        </form>
      </div>
    </div >
  );
};

export default SignUp;
