import styles from './eventpage.css';
import Header from '../../components/header/header';
import Footer from '../../components/footer/footer.jsx';
import StripeCheckout from "react-stripe-checkout";
import React, { useState, useEffect, Component } from 'react';
import { useLocation, useHistory } from "react-router-dom";
import moment from 'moment';


const Eventtypes = (props) =>
{

	let history = useHistory();
	const location = useLocation();
	const EventData = location.state;
	const [id, setid] = useState();
	const [events, setEvents] = useState([]);
	const [title, setTitle] = useState("");
	const [isReload, setisReload] = useState(true);
	const [SearchEventData, setSearchEventData] = useState([]);
	var searchEmpty = true;
	const getEvents = async () =>
	{
		setisReload(false);
	}



	return (
		<div>

			<header>
				<Header />
				<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.14.0/css/all.min.css" integrity="sha512-1PKOgIY59xJ8Co8+NE6FZ+LOAZKjy+KY8iq0G4B3CyeY6wYHN3yt9PW0XpSriVlkMXe40PTKnXrLnZ9+fkDaog==" crossorigin="anonymous" />
			</header>

			<body class="epbody">
				<h1>Events</h1>
				<div>
					<div class="wrap">
						<div class="search">
							<input type="text" class="searchTerm" placeholder="What are you looking for?" onChange={e => setTitle(e.target.value)}></input>
							<button type="submit" class="searchButton" onClick={() => getEvents()}>
								<i class="fa fa-search"></i>
							</button>
						</div>
					</div>
				</div>
				<div class="pageheight">
					{EventData.detail.map(data =>
					{

						if ((data.title.toLowerCase()).indexOf(title.toLowerCase()) != -1 && !data.competitionStarted && !data.privacy && Date.parse(moment().format("YYYY-MM-DD HH:mm")) > Date.parse(data.registration_begin) && Date.parse(moment().format("YYYY-MM-DD HH:mm")) < Date.parse(data.registration_end))
						{
							searchEmpty = false;
							return <body>
								<div class="courses-container">
									<div class="course">
										<div class="course-preview" style={{
											backgroundImage: `url(${data.image_path})`,
										}}>
											<div class="demo-wrap">
												<div class="demo-content">
													<h6>Event Type:</h6>
													<h3>{data.type}</h3>

												</div>
											</div>
										</div>
										<div class="course-info">

											<h6>{data.organiser}</h6>
											<h2 style={{ textTransform: "capitalize" }}>{data.title}</h2>

											<table>
												<tr>
													<th>Event ID</th>
													<th>Cost</th>
													<th>Start Date</th>

												</tr>
												<tr>
													<td>{data.eventId}</td>
													<td>€{data.cost}</td>
													<td>{data.registration_end.substr(0, 10)} </td>
												</tr>
											</table>
											<div class="progress-container">
												<span class="progress-text">
													{data.numberOfParticipants}/{data.maxNumberOfParticipants} Tickets sold
												</span>
												<progress id="file" value={data.numberOfParticipants / data.maxNumberOfParticipants * 100} max="100"></progress>

											</div>

											<button class="btn" type="submit"
												onClick={() =>
												{
													history.push({
														pathname: './EventDetails',
														state: { eventId: data.eventId }
													});
												}}></button>
										</div>
									</div>
								</div>

							</body>

						}

					})}
					{searchEmpty ?
						<div><strong><div>There are no events matching your search.</div></strong></div> : null}
				</div>
			</body>
			<div>
				<footer>
					<Footer />
				</footer>
			</div>
		</div>
	);
}

export default Eventtypes;