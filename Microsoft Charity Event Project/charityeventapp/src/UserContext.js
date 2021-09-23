import React, { useState, createContext, useEffect } from 'react';


export const UserContext = createContext(null);

export const UserProvider = (props) =>
{
    const [user5, setUser5] = useState(() =>
    {
        const localdata = localStorage.getItem("User");
        return localdata ? localdata : "";
    });
    useEffect(() =>
    {
        localStorage.setItem("User", user5);
    }, [user5]);


    const [name, setName] = useState(() =>
    {
        const localdata = localStorage.getItem("Type");
        return localdata ? localdata : "";
    });
    useEffect(() =>
    {
        localStorage.setItem("Type", name);
    }, [name]);

    const [verified, setVerified] = useState(() =>
    {
        const localdata = localStorage.getItem("verified");
        return localdata ? localdata : "";
    });

    const [tabOpen, setTabOpen] = useState(() =>
    {
        const localdata = localStorage.getItem("tabOpen");
        return localdata ? localdata : "";
    });

    useEffect(() =>
    {
        localStorage.setItem("tabOpen", tabOpen);
    }, [tabOpen]);

    useEffect(() =>
    {
        localStorage.setItem("verified", verified);
    }, [verified]);


    //Deployment Ips (use these when pushing to deployment)
    const url = "http://20.101.8.254";
    const urlFrontEnd = url;   //(used for sending invite link)


    // //Development Ips (use these when working locally)
    // const urlFrontEnd = "http://localhost:3000";
    // const url = "http://localhost:5000";   //(used for sending invite link)


    return (

        <UserContext.Provider value={{ user5, setUser5, name, setName, verified, setVerified, tabOpen, setTabOpen, url, urlFrontEnd }}>
            {props.children}
        </UserContext.Provider>
    );
}
