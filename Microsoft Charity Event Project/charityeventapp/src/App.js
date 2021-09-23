import { BrowserRouter as Router, Route, Switch } from "react-router-dom";
import React, { useEffect, useState, useContext } from 'react'
import Recaptcha from "react-recaptcha";
import Login from './pages/LoginPages/Login';
import SignUp from './pages/LoginPages/SignUp';
import Home from './pages/Home/home';
import CreateEvent from "./pages/CreateEvent/createEvent";
import About from './pages/AboutUs/about';
import Profile from './pages/Profile/profile';
import EventTypes from './pages/EventTypes/Eventtypes';
import { UserContext } from "./UserContext";
import EventDetails from "./pages/EventTypes/EventDetails";
import Contact from "./pages/AboutUs/Contact";
import ManageEvent from "./pages/EventTypes/ManageEvent";
import ResetPassword from "./pages/ResetPassword/ResetPassword";

function App()
{

  const { user5, setUser5, name, setName, verified, setVerified, url } = useContext(UserContext);

  //const userLogin = "";

  useEffect(() =>
  {
    (
      async () =>
      {
        const response = await fetch(""+url+"/api/user/" + user5 + "", {
          method: "GET",
          header: { "Content-Type": "application/json" },
          credentials: "include"
        });

        if (response.ok)
        {
          const user = await response.json();
          setName(user.email);
          setVerified(user.verified);
        }
      }
    )();
  }, [user5, name, verified]);



  return (
    <Router>
      <Switch>
        <Route exact path="/" component={Home} />
        <Route path="/Home" exact component={Home} />
        <Route path="/Login" component={Login} />
        <Route path="/SignUp" component={SignUp} />
        <Route path="/About" component={About} />
        <Route path="/CreateEvent" component={CreateEvent} />
        <Route path="/Profile" component={Profile} />
        <Route path="/EventTypes" component={EventTypes} />
        <Route path="/ManageEvents" component={ManageEvent} />
        <Route path="/EventDetails" component={EventDetails} />
        <Route path="/Contact" component={Contact} />
        <Route path="/ResetPassword" component={ResetPassword} />
      </Switch>
    </Router>
  );
}

export default App;


