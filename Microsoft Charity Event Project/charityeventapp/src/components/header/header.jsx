import React, { useState, useContext } from 'react';
import './header.css';
import { useHistory } from "react-router-dom";
import { Redirect } from "react-router-dom";
import Home from '../../pages/Home/home';
import { UserContext } from "../../UserContext";
import logo from './logo.png'

function Header(props)
{
    let history = useHistory();
    const [redirect, setRedirect] = useState(false);
    const { name, setName, user5, setUser5, verified, setVerified, url } = useContext(UserContext);
    let toshow;

    const logout = async () =>
    {
        const response = await fetch("" + url + "/api/logout", {
            method: "POST",
            header: { "Content-Type": "application/json" },
            credentials: "include"
        });

        if (response.ok)
        {
            setName("");
            setUser5("");
            setVerified("");
            history.push("./home");
        } else
        {
            console.log("Error");
        }
    }


    if (user5 === "Organiser" & (verified === "true" || verified))
    {
        toshow = (
            <ul class="navbar-nav ml-auto">

                <li class="nav-item">
                    <a class="nav-link" onClick={() => { history.push("./Profile"); }}>Profile</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" onClick={() => { history.push("./CreateEvent"); }}>Create Event</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" onClick={logout}>Logout</a>
                </li>
            </ul>
        )
    } else if (user5 === "Organiser" & (verified === "false" || !verified))
    {
        console.log("not verified and " + user5);
        toshow = (
            <ul class="navbar-nav ml-auto">
                <li class="nav-item">
                    <a class="nav-link" onClick={() => { history.push("./Profile"); }}>Profile</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" onClick={logout}>Logout</a>
                </li>
            </ul>
        )
    }

    else if (user5 === "Participant")
    {
        toshow = (
            <ul class="navbar-nav ml-auto">
                <li class="nav-item">
                    <a class="nav-link" onClick={() => { history.push("./Profile"); }}>Profile</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" onClick={logout}>Logout</a>
                </li>
            </ul>
        )
    }
    else if (name === "")
    {

        toshow = (
            <ul class="navbar-nav ml-auto">
                <li class="nav-item">
                    <a class="nav-link" onClick={() => { history.push("./About"); }}>About</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" onClick={() => { history.push("./Login"); }}>Sign in</a>
                </li>
            </ul>
        )

    } else
    {
        console.log("ERROR")
    }

    return (
        <div >
            <nav class="navbar navbar-expand-lg navbar-dark bg-dark fixed-top">
                <div class="container">
                    <a href="/Home"><img class="navbar-brand" src={logo} alt="CharityEvent" width="200" height="40"></img></a>
                    <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarResponsive" aria-controls="navbarResponsive" aria-expanded="false" aria-label="Toggle navigation">
                        <span class="navbar-toggler-icon"></span>
                    </button>
                    <div class="collapse navbar-collapse" id="navbarResponsive">
                        <ul class="navbar-nav ml-auto">
                            <a class="nav-link" id="accounttype">{user5} </a>
                            {toshow}
                        </ul>
                    </div>
                </div>
            </nav>
            <link rel="stylesheet" href="styles.css"></link>
            <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/css/bootstrap.min.css" integrity="sha384-MCw98/SFnGE8fJT3GXwEOngsV7Zt27NXFoaoApmYm81iuXoPkFOJwJ8ERdknLPMO" crossorigin="anonymous"></link>
            <link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.2.0/css/all.css" integrity="sha384-hWVjflwFxL6sNzntih27bfxkr27PmbbK/iSvJ+a4+0owXq79v+lsFkW54bOGbiDQ" crossorigin="anonymous"></link>

            Welcome
        </div>
    );
}

export default Header