import styles from "./createEvent.css";
import Header from "../../components/header/header";
import Footer from "../../components/footer/footer.jsx";
import "@pathofdev/react-tag-input/build/index.css";
import Recaptcha from "react-recaptcha";
import ReCAPTCHA from "react-google-recaptcha";
import ReactTagInput from "@pathofdev/react-tag-input";
import React, { Component, useState, useContext, useEffect } from "react";
import moment from 'moment';
import { Redirect, useHistory } from "react-router-dom";
import { UserContext } from "../../UserContext";
import ImageUploader from "react-images-upload";
import { tsConstructorType } from "@babel/types";
import InnerHTML from 'dangerously-set-html-content'
import firebase from "../../firebase"
import storage from '../../firebase';

var IBAN = require("iban");

const CreateEvent = (props) =>
{
  let history = useHistory();
  const { user5, setUser5, name, setName, url } = useContext(UserContext);
  const [Title, setTitle] = useState("");
  const [Type, setType] = useState("Competitions");
  const [Cost, setCost] = useState(0.0);
  const [Registration_begin, setRegistration_begin] = useState(moment().format("YYYY-MM-DDThh:mm"));
  const [Registration_end, setRegistration_end] = useState();
  const [checkprivacy, setCheckprivacy] = useState(0);
  const [Privacy, setPrivacy] = useState(true);
  const [IBAN, setIBAN] = useState("");
  const [Description, setDescription] = useState("");
  const [MaxNumberOfParticipants, setMaxNumberOfParticipants] = useState();
  const [Split, setSplit] = useState(50);
  const [AvailableSelections, setAvailableSelections] = useState([]);
  const [Redirect, setRedirect] = useState(false);
  const [AreTermsAccepted, setAreTermsAccepted] = useState(false);
  const [pictures, setPictures] = useState();
  const [isVerified, setisVerified] = useState(false);

  const [image, setImage] = useState("");
  const [imageUrl, setImageUrl] = useState(undefined);


  var Image_path = "";
  const [getid, setId] = useState("competitions.jpg");







  function recaptchaLoaded()
  {

  }

  function verifyCallback(response)
  {
    if (response)
    {
      setisVerified(true);
    }
  }



  const submit = (e) =>
  {

    e.preventDefault();



    if (AreTermsAccepted)
    {

      Image_path = "https://firebasestorage.googleapis.com/v0/b/charityevent-831c9.appspot.com/o/images%2F" + getid + "?alt=media&token=83cad076-133b-4136-a5fc-2d19be43684e";
      if (AvailableSelections.length === MaxNumberOfParticipants)
      {
        const event = {
          "Email": name,
          Title,
          Type,
          Cost,
          Registration_begin,
          Registration_end,
          "Privacy": Boolean(parseInt(checkprivacy)),
          IBAN,
          Image_path,
          Description,
          MaxNumberOfParticipants,
          PayoutSplitPercentageForWinner: Split,
          AvailableSelections,
        };

        fetch("" + url + "/api/CreateEvent", {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          credentials: "include",
          body: JSON.stringify(event),
        })
          .then((res) =>
          {
            if (!res.ok)
            {
              document.getElementById('errfn').innerHTML = "Selections should be unique";
              setTimeout(function () { clearText(); }, 5000);

            } else
            {
              upload();
              setRedirect(true);

            }
          })
          .catch((err) =>
          {

          });
      } else
      {
        document.getElementById('errfn').innerHTML = "There should be one selection per participant";
        setTimeout(function () { clearText(); }, 5000);

      }
    } else
    {
      document.getElementById('errfn').innerHTML = "Please accept terms and conditions.";
      setTimeout(function () { clearText(); }, 5000);
    }


  };

  if (Redirect)
  {
    history.push("./home");
  }

  function clearText()
  {
    document.getElementById('errfn').innerHTML = " ";
  }

  const upload = () =>
  {

    if (image === "" || image == null)
    {

    } else if (getid != "competitions.jpg" || getid != "football.png" || getid != "esport.jpg" || getid != "olympics.jpg")
    {
      storage.ref(`images/${getid}`).put(image);
    }

    setRedirect(true);
  }

  function uploadImage(e)
  {
    setImage(e.target.files[0])
    var crypto = require("crypto");
    setId(crypto.randomBytes(20).toString('hex'));
  }


  if (user5 != "Organiser")
  {
    history.push("./home");
  }


  const updateImage = (e) =>
  {
    setType(e)
    if (e === "Football")
    {
      setId("football.png");
    } else if (e === "Olympics")
    {
      setId("olympics.jpg");
    } else if (e === "eSports")
    {
      setId("esport.jpg");
    } else if (e === "Competitions")
    {
      setId("competitions.jpg");
    }
  }


  return (
    <div>
      <div>
        <header>
          <Header name={props.name} setName={props.setName} />
          <link rel="stylesheet" href="./createEvent.css"></link>

        </header>
      </div>
      <div>
        <body class="cebody">
          <div class="createEventcontainer">
            <form class="ceform" onSubmit={submit} onsubmit="return false">
              <div class="row">
                <h2 class="ceh2">
                  Create your event- It's simple, easy and fast
                </h2>
                <div class="input-group input-group-icon">
                  <input
                    class="ceinput"
                    type="text"
                    placeholder="Add a catchy title for your event"
                    onChange={(e) => setTitle(e.target.value)}
                    required
                    pattern="^[A-Za-z0-9_ ]{5,30}$"
                    title="Event title can only contain Alpha-numeric characters: A-Z, a-z, 0-9 & Must be between 5-30 characters."
                  />
                  <div class="input-icon">
                    <i class="fa fa-id-card"></i>
                  </div>
                </div>
              </div>
              <div class="row">
                <h6 class="ceh6">Event Type & Privacy</h6>
                <div class="input-group">
                  <div class="input-group">
                    <select onChange={(e) => updateImage(e.target.value)}>
                      <option>Competitions</option>
                      <option>Olympics</option>
                      <option>Football</option>
                      <option>eSports</option>
                    </select>
                    <select onChange={(e) => setCheckprivacy(e.target.value)}>
                      <option value={0}>Public</option>
                      <option value={1}>Private</option>
                    </select>
                  </div>
                </div>
              </div>
              <div class="row">
                <h6 class="ceh6">Event description</h6>
                <div class="input-group input-group-icon">
                  <input
                    class="ceinput input-group"
                    type="textarea"
                    placeholder="Write about your event and why it's awesome!"
                    onChange={(e) => setDescription(e.target.value)}
                    required
                    pattern="^.{10,150}$"
                    title="Description must be between 10 and 300 Characters long."
                  />
                  <div class="input-icon">
                    <i class="fa fa-info-circle"></i>
                  </div>
                </div>
              </div>
              {pictures}
              <div class="row">

                <div>
                  <h6 class="ceh6">Event image</h6>
                  <div class="uploadImage">
                    <input class="child" type="file" accept="image/png, image/jpeg" onChange={(e) => { uploadImage(e) }} />
                  </div>
                </div>
              </div>
              <br>
              </br>
              <div class="row">
                <div class="absolute">
                  <h6 class="ceh6">Registration timings:</h6>
                  <div class="col-half">
                    <h8>Starts at</h8>

                    <input
                      class="ceinput input-group"
                      type="datetime-local"
                      placeholder="Starts at"
                      onChange={(e) => setRegistration_begin(e.target.value)}
                      required
                      min={moment().format("YYYY-MM-DDThh:mm")}
                    />

                  </div>

                  <div class="col-half">
                    <div class="addspacedate"> -
                      <h10>Ends at</h10>

                      <input
                        class="ceinput input-group"
                        type="datetime-local"
                        placeholder="Starts at"
                        onChange={(e) => setRegistration_end(e.target.value)}
                        required
                        min={Registration_begin}
                      />
                    </div>
                  </div>
                </div>
              </div>

              <div class="row">
                <h6 class="ceh6">Max Limit (Participants)</h6>
                <div class="input-group input-group-icon">
                  <input
                    class="ceinput input-group"
                    type="number"
                    placeholder="Enter amount here.."
                    onChange={(e) =>
                      setMaxNumberOfParticipants(parseInt(e.target.value))
                    }
                    required
                    pattern="^([1-9][0-9]{0,2}|1000)$"
                    min="2"
                    max="1000"
                    title="Event must have between 2 and 1000 participants"
                  />
                  <div class="input-icon">
                    <i class="fas fa-users"></i>
                  </div>
                </div>
              </div>
              <div class="row">
                <h6 class="ceh6">Ticket price</h6>
                <div class="input-group input-group-icon">
                  <input
                    class="ceinput"
                    type="number"
                    step="0.01"
                    min="0"
                    max="9999.99"
                    placeholder="Enter amount here.."
                    onChange={(e) => setCost(parseFloat(e.target.value))}
                    required
                    pattern="^([1-9][0-9]{0,2}|1000)$"
                    title="Event must have between 1 and 1000 participants"
                  />
                  <div class="input-icon">
                    <i class="far fa-money-bill-alt"></i>
                  </div>
                </div>
              </div>
              <div class="row" title="Determine what percentage of the prize pool will go to the winner of the competition." />
              <div class="row">
                <h6 class="ceh6">Prize Split: &nbsp;&nbsp;&nbsp;&nbsp; <br></br>Organiser's share : {Split}%  &nbsp;&nbsp;&nbsp;  Winner's Share: {100 - Split}%</h6>
                <input class="ceinput"
                  type="range"
                  min="0"
                  max="100"
                  defaultValue="50"
                  name="split"
                  id="prize"
                  oninput="prize_value.value=prize.value"
                  onChange={(e) => setSplit(parseInt(e.target.value))}
                  required
                />

              </div>
              <div class="row" title="Enter the selections you wish to make available for your competition.">
                <h6 class="ceh6">Available Selections</h6>
                <div class="ceinput input-group input-group-icon">
                  <ReactTagInput
                    title="Enter the picks for participants"
                    tags={AvailableSelections}
                    onChange={(newTags) => setAvailableSelections(newTags)}
                  />
                </div>
                <div>
                  <div id="errfn" class="errfn">   </div>
                </div>
              </div>
              <div class="row">
                <h6 class="ceh6">IBAN</h6>
                <div class="input-group input-group-icon">
                  <input
                    class="ceinput input-group"
                    type="test"
                    id="iban"
                    placeholder="IBAN"
                    onChange={(e) => setIBAN(e.target.value)}
                    required
                    pattern="((NO)[0-9A-Z]{2}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}[ ][0-9A-Z]{3}|(NO)[0-9A-Z]{13}|(BE)[0-9A-Z]{2}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}|(BE)[0-9A-Z]{14}|(DK|FO|FI|GL|NL)[0-9A-Z]{2}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}[ ][0-9A-Z]{2}|(DK|FO|FI|GL|NL)[0-9A-Z]{16}|(MK|SI)[0-9A-Z]{2}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}[ ][0-9A-Z]{3}|(MK|SI)[0-9A-Z]{17}|(BA|EE|KZ|LT|LU|AT)[0-9A-Z]{2}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}|(BA|EE|KZ|LT|LU|AT)[0-9A-Z]{18}|(HR|LI|LV|CH)[0-9A-Z]{2}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}[ ][0-9A-Z]{1}|(HR|LI|LV|CH)[0-9A-Z]{19}|(BG|DE|IE|ME|RS|GB)[0-9A-Z]{2}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}[ ][0-9A-Z]{2}|(BG|DE|IE|ME|RS|GB)[0-9A-Z]{20}|(GI|IL)[0-9A-Z]{2}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}[ ][0-9A-Z]{3}|(GI|IL)[0-9A-Z]{21}|(AD|CZ|SA|RO|SK|ES|SE|TN)[0-9A-Z]{2}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}|(AD|CZ|SA|RO|SK|ES|SE|TN)[0-9A-Z]{22}|(PT)[0-9A-Z]{2}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}[ ][0-9A-Z]{1}|(PT)[0-9A-Z]{23}|(IS|TR)[0-9A-Z]{2}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}[ ][0-9A-Z]{2}|(IS|TR)[0-9A-Z]{24}|(FR|GR|IT|MC|SM)[0-9A-Z]{2}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}[ ][0-9A-Z]{3}|(FR|GR|IT|MC|SM)[0-9A-Z]{25}|(AL|CY|HU|LB|PL)[0-9A-Z]{2}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}|(AL|CY|HU|LB|PL)[0-9A-Z]{26}|(MU)[0-9A-Z]{2}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}[ ][0-9A-Z]{2}|(MU)[0-9A-Z]{28}|(MT)[0-9A-Z]{2}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}[ ][0-9A-Z]{4}[ ][0-9A-Z]{3}|(MT)[0-9A-Z]{29})"
                    title="Enter a valid IBAN number (IE12 AIBK 1111 2222 3333 44)"
                  />
                  <div class="input-icon">
                    <i class="fas fa-shield-alt" aria-hidden="true"></i>
                  </div>
                </div>
              </div>
              <div class="row">
                <div>
                  <div id="errfn" class="errfn">   </div>
                </div>
                <h6 class="ceh6">Terms and Conditions</h6>
                <div class="input-group">

                  <label for="terms" style={{ color: "black" }}> <input id="terms" type="checkbox" onChange={(e) => setAreTermsAccepted(e.target.value)} /> &nbsp;
                    I accept the terms and conditions for hosting an event
                    through &nbsp; &nbsp;ThunderBirds, and hereby confirm I have read the
                    privacy policy.
                  </label>
                </div>
              </div>

              <Recaptcha
                sitekey="6LdsCJMbAAAAAIj4xLcX70tvaMlSo8qmNNboUvxt"
                render="explicit"
                onloadCallback={recaptchaLoaded}
                verifyCallback={verifyCallback}
              />

              <div class="row">
                <button class="button" type="submit">
                  Submit
                </button>

              </div>

            </form>
          </div>
        </body>
      </div>
      <div>
        <footer>
          <Footer />{" "}
        </footer>
      </div>
    </div>
  );
};

export default CreateEvent;