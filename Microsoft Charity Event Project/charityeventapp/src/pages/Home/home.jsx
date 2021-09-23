import styles from './Home.css'
import Header from '../../components/header/header'
import Footer from '../../components/footer/footer.jsx'
import football from './football.png'
import olympics from './olympics.jpg'
import esport from './eSports.jpeg'
import competitions from './competitions.jpg'
import e1 from './e1.png'
import e2 from './e2.jpg'
import e3 from './e3.png'
import React, { useState, useEffect, useContext } from 'react'
import { useHistory } from "react-router-dom";
import StripeCheckout from "react-stripe-checkout";
import InnerHTML from 'dangerously-set-html-content'
import { UserContext } from "../../UserContext";
import { render } from '@testing-library/react'
import Popup from '../../components/popup/popup';
//const url = "http://localhost:5000/api/getevents/";

const Home = () =>
{
	const [redirect, setRedirect] = useState(false);
	const [EventTypeData, setEventTypeData] = useState([]);
	const { name, setName, user5, setUser5, url, verified } = useContext(UserContext);
	const [displayed, setdisplayed] = useState(true);

	let history = useHistory();

	const charityEventType = async (value) =>
	{
		const response = await fetch("" + url + "/api/getevents/" + value, {
			method: "GET",
			header: { "Content-Type": "application/json" }
		});

		if (response.ok)
		{
			setEventTypeData(await response.json());
			setRedirect(true);
		} else
		{

		}


	}





	if (redirect)
	{
		history.push({ pathname: "./EventTypes", state: { detail: EventTypeData } });
	}

	const html = `
    
    <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js" integrity="sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo" crossOrigin="anonymous"></script>
			<script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.3/umd/popper.min.js" integrity="sha384-ZMP7rVo3mIykV+2+9J3UJ46jBk0WLaUAdn689aCwoqbBJiSnjAK/l8WvCWPIPm49" crossOrigin="anonymous"></script>
			<script src="https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/js/bootstrap.min.js" integrity="sha384-ChfqqxuZUCnJSK3+MXmPNIyE6ZbWh2IMqE241rYiqJxyMiZ6OW/JmZQ5stwEULTy" crossOrigin="anonymous"></script>
			<script src="https://kit.fontawesome.com/355af1737a.js" crossorigin="anonymous"></script>
			<script async src="https://www.googletagmanager.com/gtag/js?id=G-FTBBPQX288%22%3E</script> <script>   window.dataLayer = window.dataLayer || [];   function gtag(){dataLayer.push(arguments);}   gtag('js', new Date());    gtag('config', 'G-FTBBPQX288'); </script>
			<script src="https://www.googleoptimize.com/optimize.js?id=OPT-NBC82H6"></script>
            <script>(function(a,s,y,n,c,h,i,d,e){s.className+=' '+y;h.start=1*new Date;
            h.end=i=function(){s.className=s.className.replace(RegExp(' ?'+y),'')};
            (a[n]=a[n]||[]).hide=h;setTimeout(function(){i();h.end=null},c);h.timeout=c;
            })(window,document.documentElement,'async-hide','dataLayer',4000,
            {'OPT-NBC82H6':true});</script>
			`





	return (
		<div>
			<div>
				<header>
					<Header />

					<link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/css/bootstrap.min.css" integrity="sha384-MCw98/SFnGE8fJT3GXwEOngsV7Zt27NXFoaoApmYm81iuXoPkFOJwJ8ERdknLPMO" crossOrigin="anonymous"></link>
					<link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.2.0/css/all.css" integrity="sha384-hWVjflwFxL6sNzntih27bfxkr27PmbbK/iSvJ+a4+0owXq79v+lsFkW54bOGbiDQ" crossOrigin="anonymous"></link>

					<InnerHTML html={html} />


				</header>
			</div>
			<div>

				<body>
					<div>
						<div id="carouselExampleIndicators" class="carousel slide" data-ride="carousel">
							<ol class="carousel-indicators">
								<li data-target="#carouselExampleIndicators" data-slide-to="0" class="active"></li>
								<li data-target="#carouselExampleIndicators" data-slide-to="1"></li>
								<li data-target="#carouselExampleIndicators" data-slide-to="2"></li>
							</ol>
							<div class="carousel-inner" role="listbox">
								{/*Slide One - Set the background image for this slide in the line below*/}
								<div class="carousel-item active" style={{ backgroundImage: `url(${e1})` }}> {/*
							<div class="carousel-caption d-none d-md-block">
								<h3 class="display-4">First Slide</h3>
								<p class="lead">This is a description for the first slide.</p>
							</div>*/} </div>
								{/* Slide Two - Set the background image for this slide in the line below */}
								<div class="carousel-item" style={{ backgroundImage: `url(${e2})` }}> {/*
							<div class="carousel-caption d-none d-md-block">
								<h3 class="display-4">Second Slide</h3>
								<p class="lead">This is a description for the second slide.</p>
							</div>*/}
								</div>
								{/* Slide Three - Set the background image for this slide in the line below */}
								<div class="carousel-item" style={{ backgroundImage: `url(${e3})` }}> {/*
							<div class="carousel-caption d-none d-md-block">
								<h3 class="display-4">Third Slide</h3>
								<p class="lead">This is a description for the third slide.</p>
							</div>*/} </div>
							</div>
							<a class="carousel-control-prev" href="#carouselExampleIndicators" role="button" data-slide="prev"> <span class="carousel-control-prev-icon" aria-hidden="true"></span> <span class="sr-only">Previous</span> </a>
							<a class="carousel-control-next" href="#carouselExampleIndicators" role="button" data-slide="next"> <span class="carousel-control-next-icon" aria-hidden="true"></span> <span class="sr-only">Next</span> </a>
						</div>
					</div>
					<br></br>
					<div>
						<h1 class="hh1" style={{ backgroundcolor: "#263238" }}>Online Events </h1>

						<div class="cards-list">
							<div class="homecard 1" onClick={() => charityEventType("Olympics")}>
								<div class="card_image"> <img src={olympics} /> </div>
								<div class="card_title title-white">
									<p>Olympics</p>
								</div>
							</div>
							<div class="homecard 2" onClick={() => charityEventType("Football")}>
								<div class="card_image"> <img src={football} /> </div>
								<div class="card_title title-white">
									<p>Football</p>
								</div>
							</div>
							<div class="homecard 3" onClick={() => charityEventType("eSports")}>
								<div class="card_image"> <img src={esport} /> </div>
								<div class="card_title title-white">
									<p>eSports</p>
								</div>
							</div>
							<div class="homecard 4" onClick={() => charityEventType("Competitions")}>
								<div class="card_image"> <img src={competitions} /> </div>
								<div class="card_title title-white">
									<p>Competitions</p>
								</div>
							</div>
						</div>
					</div>
					<br></br>

					<br></br>

					{user5 === "Organiser" && (verified === "false" || !verified) && displayed &&
						<Popup closePopup={setdisplayed}
							content={<>
								<a>Your account is not verified, go to the profile page and verify your account before you can create events.</a>
							</>}
						/>}

				</body>
			</div>
			<div>
				<footer>
					<Footer /> </footer>
			</div>
		</div>
	);
}

export default Home;