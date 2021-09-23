import React, { useState, useEffect, useContext } from 'react'
import 'bootstrap/dist/css/bootstrap.min.css';
import EventImage from "./Last_Man_Standing.png";
import Header from '../../components/header/header';
import Footer from '../../components/footer/footer.jsx';
import StripeCheckout from "react-stripe-checkout";
import { Redirect, useLocation, useHistory } from 'react-router-dom';
import { UserContext } from "../../UserContext";
import PopupAutoClose from '../../components/popup/popupAutoClose';
import moment from 'moment';

var EventId;
var inviteLink = "";
const EventDetails = (props) =>
{
  let history = useHistory();
  const [Event, setEvent] = useState("");
  const [Selections, setSelections] = useState([]);
  const [userpick, setUserpick] = useState("");
  const location = useLocation();
  const [PickedSelections, setPickedSelections] = useState([]);
  const { user5, setUser5, name, setName, verified, setVerified, url } = useContext(UserContext);
  const [date, setDate] = useState();
  const [time, setTime] = useState();
  const prizePool = Math.round(((Event.cost * Event.payoutSplitPercentageForWinner) / 100) * Event.numberOfParticipants).toFixed(2);
  const [Limit, setLimit] = useState(false);
  const [EventStarted, setStartEvent] = useState(false);
  var inviteLink = getQueryVariable('inviteLink');
  const [registered, setregistered] = useState(false);
  const [registeredFail, setregisteredFail] = useState(false);



  const displayPickError = () =>
  {

    document.getElementById('errfn').innerHTML = "Selection cannot be empty.";
    setTimeout(function () { clearText(); }, 5000);

  }



  if (typeof (location.state) !== 'undefined' && location.state != null)
  {
    EventId = location.state.eventId;

  }

  useEffect(() =>
  {

    if (inviteLink != "")
    {
      (
        async () =>
        {
          const response = await fetch("" + url + "/api/geteventid/" + inviteLink + "", {
            method: "GET",
            header: { "Content-Type": "application/json" },
            credentials: "include"
          });

          if (response.ok)
          {
            const event = await response.json();
            EventId = event;
          }
        }
      )();
    }
    setTimeout(function ()
    {
      (
        async () =>
        {
          const response = await fetch("" + url + "/api/geteventpage/" + EventId + "", {
            method: "GET",
            header: { "Content-Type": "application/json" },
            credentials: "include"
          });

          if (response.ok)
          {
            const event = await response.json();
            setEvent(event);
            setSelections(event.availableSelections);
            setDate(event.registration_end.substr(0, 10));
            setTime(event.registration_end.substr(11, 12));
            if (event.maxNumberOfParticipants > event.numberOfParticipants)
            {
              setLimit(true);
            }
            if (event.maxNumberOfParticipants === event.numberOfParticipants + 1)
            {
              setStartEvent(true);
            }
          }
        }
      )();
      (
        async () =>
        {
          const response = await fetch("" + url + "/api/getPickedSelections/" + EventId + "", {
            method: "GET",
            header: { "Content-Type": "application/json" },
            credentials: "include"
          });

          if (response.ok)
          {
            const list = await response.json();
            setPickedSelections(list);
          } else
          {
          }
        }
      )();
    }, 600);

  }, [location, verified]);


  const startEvent = (eventId) =>
  {

    if (EventStarted)
    {
      setStartEvent(true)
      fetch("" + url + "/api/startEvent/" + eventId + "", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        credentials: "include",
      }).then(res =>
      {
      }).catch(err =>
      {

      });
    }


  }




  async function tokenHandler(token)
  {

    if (userpick != "")
    {
      if (token.email === name)
      {
        if (Limit)
        {
          const register = {
            "Email": name,
            "EventTitle": Event.title,
            "Price": Event.cost,
            "EventId": EventId,
            "Selection": userpick
          }
          fetch("" + url + "/api/TicketGenerate", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            credentials: "include",
            body: JSON.stringify(register)
          }).then(res =>
          {

            if (!res.ok)
            {
              setregisteredFail(true);

            }
            else
            {
              setregistered(true);
              if (EventStarted)
              {
                startEvent(EventId)
              }
              window.location.reload(true);
            }
          }).catch(err =>
          {

          });
        }
      } else
      {
        document.getElementById('errfn').innerHTML = "Please enter a valid email when paying.";
        setTimeout(function () { clearText(); }, 5000);
      }
    } else
    {
      document.getElementById('errfn').innerHTML = "Selection cannot be empty.";
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


  return (
    <div>
      <header>
        <Header />
      </header>

      <body class="epbody">
        <div class="pageheight">
          <div class="row">
            <div class="col-md-6 mb-4 mb-md-0">

              <div id="mdb-lightbox-ui"></div>

              <div class="mdb-lightbox">

                <div class="row product-gallery mx-1">


                  <div class="col-16 mb-0">
                    <figure class="main-img">
                      <img src={Event.image_path}
                        class="img-fluid z-depth-1" />
                    </figure>

                  </div>
                </div>

              </div>

            </div>
            <div class="col-md-6">

              <h2 style={{ font: "bold" }}>{Event.title}</h2>
              <p class="mb-2 text-muted text-uppercase small">Ticket Price</p>
              <p><span class="mr-1"><strong>€{Event.cost}</strong></span></p>
              <p class="pt-1">{Event.description}</p>
              <div class="">
                <table class="table table-sm table-borderless mb-0">
                  <tbody>
                    <tr>
                      <th class="pl-0 w-25" scope="row"><strong>Organiser Name</strong></th>
                      <td>{Event.organiserName}</td>
                    </tr>
                    <tr>
                      <th class="pl-0 w-25" scope="row"><strong>Event Date</strong></th>
                      <td>{date}</td>
                    </tr>
                    <tr>
                      <th class="pl-0 w-25" scope="row"><strong>Time</strong></th>
                      <td>{time}</td>
                    </tr>
                    <tr>
                      <th class="pl-0 w-25" scope="row"><strong>Location</strong></th>
                      <td>Online</td>
                    </tr>
                    <tr>
                      <th class="pl-0 w-25" scope="row"><strong>Current Prize</strong></th>
                      <td>€{prizePool}</td>
                    </tr>
                  </tbody>
                </table>
              </div>
              <hr />
              <div class="mb-2">{user5 === "Participant" && Limit && !Event.competitionStarted && !Event.competitionCompleted ?
                <table class="table table-sm table-borderless">
                  <tbody>

                    <tr>
                      <th class="pl-0 w-25" scope="row"><strong>Select your pick</strong></th>
                      <td>

                        <div class="mt-1">
                          <select onChange={e => setUserpick(e.target.value)}>
                            <option value=""></option>
                            {Selections.filter(n => !PickedSelections.includes(n)).map(data =>
                              <option value={data}>{data}</option>)}
                          </select>
                        </div>

                      </td>

                    </tr>

                  </tbody>

                </table> : null

              }
              </div>
              {!Event.competitionStarted && !Event.competitionCompleted ? <h5>Tickets Sold : {Event.numberOfParticipants}/{Event.maxNumberOfParticipants}
                <br></br>
                <progress id="file" value={Event.numberOfParticipants / Event.maxNumberOfParticipants * 100} max="100"></progress>
              </h5> : <h5>Registration for this event has closed!</h5>}
              {user5 != "Participant" ?
                <div><strong>You have to be signed in to a participant account to register for this event!</strong></div> : null}

              <div>
                {user5 === "Participant" ?
                  <h5>
                    {Limit && moment().format("YYYY-MM-DDThh:mm") < Event.registration_end && !Event.competitionStarted && !Event.competitionCompleted ?
                      userpick != "" ?
                        <StripeCheckout
                          stripeKey="pk_test_51J5U5RIdLNf0k1sMf2vbL2pcDxpFaBsW4BGFPfNfFdDTGO0dz7Tg0w9ouDJfM42ME3KK05suE6uYR5c3aYVGSpNL00BBeQVGCg"
                          token={tokenHandler}
                          amount={Event.cost * 100}
                          email={name}>
                          <button class="button" >Register</button>
                          <div>
                            <div id="errfn" class="errfn"> </div>
                          </div>
                        </StripeCheckout>
                        : <div>
                          <button class="button" onClick={displayPickError}>Register</button>
                          <div>
                            <div id="errfn" class="errfn"> </div>
                          </div>


                        </div>
                      : null}
                  </h5> : null}


              </div>
            </div>
          </div>
        </div>
      </body>
      <div>

        {registered &&
          <PopupAutoClose closePopup={setregistered}
            content={<>
              <a>Successfully Joined Event.</a>
            </>}
          />}

        {registeredFail &&
          <PopupAutoClose closePopup={setregisteredFail}
            content={<>
              <a>Error Joining Event.</a>
            </>}
          />}
        <footer>
          <Footer />
        </footer>
      </div>
    </div>
  )
}

export default EventDetails