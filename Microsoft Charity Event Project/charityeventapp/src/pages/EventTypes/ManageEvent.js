import styles from './ManageEvent.css';
import Header from '../../components/header/header';
import Footer from '../../components/footer/footer.jsx';
import StripeCheckout from "react-stripe-checkout";
import React, { useState, useEffect, useContext } from 'react';
import { useLocation, useHistory } from "react-router-dom";
import { UserContext } from "../../UserContext";
import PopupAutoClose from '../../components/popup/popupAutoClose';



const ManageEvent = () =>
{
	let history = useHistory();
	const location = useLocation();
	const [ParticipantList, setParticipantList] = useState([]);
	const { name, setName, user5, setUser5, url } = useContext(UserContext);
	const [id, setId] = useState();
	let toshow;
	const [isEnd, setIsEnd] = useState(false);

	const [EliminatedPlayerPopup, setEliminatedPlayerPopup] = useState(false);
	const [DeclaredWinnerPopup, setDeclaredWinnerPopup] = useState(false);
	const [EliminatedPlayerFailPopup, setEliminatedPlayerFailPopup] = useState(false);
	const [DeclaredWinnerFailPopup, setDeclaredWinnerFailPopup] = useState(false);
	const [playersAvailable, setplayersAvailable] = useState(0);


	function numberOfPlayersAvailable(Participants)
	{
		var stillPlaying = 0;
		for (var i = 0; i < Participants.length; i++)
		{

			if (Participants[i].eliminated === false)
			{
				stillPlaying++;
			}
		}

		setplayersAvailable(stillPlaying);

	}


	useEffect(() =>
	{
		(
			async () =>
			{
				const response = await fetch("" + url + "/api/getPlayerSelections/" + location.state.eventId + "", {
					method: "GET",
					header: { "Content-Type": "application/json" },
					credentials: "include"
				});

				if (response.ok)
				{
					const list = await response.json();
					setParticipantList(list);
					numberOfPlayersAvailable(list);

				}
			}
		)();
		(
			async () =>
			{
				const response = await fetch("" + url + "/api/checkeventstatus/" + location.state.eventId + "", {
					method: "GET",
					header: { "Content-Type": "application/json" },
					credentials: "include"
				});

				if (response.ok)
				{
					setIsEnd(true);

				}
			}
		)();
	}, [location]);



	function hideEmail(email)
	{
		var start = email.charAt(0);
		var emailLength = email.split('@')[0].length;

		for (var i = 0; i < emailLength - 2; i++)
		{
			start += "*";
		}
		start += email.split('@')[0].charAt(emailLength - 1);
		start += "@" + email.split('@')[1];

		return start;
	}

	function handleEliminate(id, em)
	{

		const eliminate = {
			"TicketId": id,
			"EventId": location.state.eventId
		}
		fetch("" + url + "/api/EliminatePlayer", {
			method: "POST",
			headers: { "Content-Type": "application/json" },
			credentials: "include",
			body: JSON.stringify(eliminate)
		}).then(res =>
		{
			if (!res.ok)
			{
				setEliminatedPlayerFailPopup(true);
			}
			else
			{
				setEliminatedPlayerPopup(true);
				const notify = {
					"TicketId": id,
					"EventId": location.state.eventId,
					"Email": em
				}
				fetch("" + url + "/api/notifyEliminatedParticipant", {
					method: "POST",
					headers: { "Content-Type": "application/json" },
					credentials: "include",
					body: JSON.stringify(notify)
				}).then(res =>
				{
					if (!res.ok)
					{

					}
					else
					{

						window.location.reload(true);
					}
				}).catch(err =>
				{

				});
				//window.location.reload(true);
			}
		}).catch(err =>
		{

		});
	}

	function handleDeclare(id)
	{

		const winner = {
			"TicketId": parseInt(id[0]),
			"EventId": location.state.eventId
		}
		fetch("" + url + "/api/SelectWinner", {
			method: "POST",
			headers: { "Content-Type": "application/json" },
			credentials: "include",
			body: JSON.stringify(winner)
		}).then(res =>
		{
			if (!res.ok)
			{
				setDeclaredWinnerFailPopup(true);
			}
			else
			{
				setDeclaredWinnerPopup(true);
				const notify = {
					"TicketId": parseInt(id[0]),
					"EventId": location.state.eventId,
					"Email": id[1]
				}
				fetch("" + url + "/api/notifyWinningParticipant", {
					method: "POST",
					headers: { "Content-Type": "application/json" },
					credentials: "include",
					body: JSON.stringify(notify)
				}).then(res =>
				{
					if (!res.ok)
					{

					}
					else
					{

						window.location.reload(true);
					}
				}).catch(err =>
				{

				});

			}
		}).catch(err =>
		{

		});
	}

	if (user5 === "Participant")
	{
		toshow = (
			<table class="table table-hover">
				<thead>
					<tr>
						<th scope="col">Email</th>
						<th scope="col">Pick</th>
						<th scope="col">Eliminated?</th>
					</tr>
				</thead>
				{ParticipantList.map(data =>
					<tbody>
						<tr>
							<td>{hideEmail(data.email)}</td>
							<td>{data.selection}</td>
							<td>{data.eliminated.toString()}</td>
						</tr>
					</tbody>)}
			</table>
		)
	} else if (user5 === "Organiser")
	{
		toshow = (
			<div>
				<table class="table table-hover">
					<thead>
						<tr>
							<th scope="col">Email</th>
							<th scope="col">Pick</th>
							<th scope="col">Eliminated?</th>
						</tr>
					</thead>
					{ParticipantList.map(data =>
					{


						if (location.state.compStarted)
						{
							if (data.eliminated)
							{
								return (
									<tbody>
										<tr>
											<td>{hideEmail(data.email)}</td>
											<td>{data.selection}</td>
											<td>Eliminated</td>
										</tr>
									</tbody>
								);
							} else if (playersAvailable > 1)
							{
								return (
									<tbody>
										<tr>
											<td>{hideEmail(data.email)}</td>
											<td>{data.selection}</td>
											<td><button class="button" type="submit"
												onClick={() => handleEliminate(data.ticket_id, data.email)}>Eliminate</button></td>
										</tr>
									</tbody>
								);
							} else
							{

								return (
									<tbody>
										<tr>
											<td>{hideEmail(data.email)}</td>
											<td>{data.selection}</td>
											<td>Last player Remaining (Declare as winner)</td>
										</tr>
									</tbody>
								);

							}
						} else
						{
							return (
								<tbody>
									<tr>
										<td>{hideEmail(data.email)}</td>
										<td>{data.selection}</td>
									</tr>
								</tbody>
							);
						}

					}
					)}
				</table>

				{location.state.compStarted ? (<>	<label>Select Winner:</label>
					<div>
						<select onChange={(e) => setId((e.target.value).split(','))}>
							<option value=""></option>
							{ParticipantList.map(data =>
							{
								if (!data.eliminated)
								{
									return (
										<option value={[data.ticket_id, data.email]}>{data.selection}</option>
									)
								}
							}
							)}

						</select>
						<button class="button"
							onClick={() => handleDeclare(id)}>Declare</button>
					</div>		</>) : null}

			</div>
		)
	}


	return (
		<div>
			<div>
				<header>
					<Header />
					<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.14.0/css/all.min.css" integrity="sha512-1PKOgIY59xJ8Co8+NE6FZ+LOAZKjy+KY8iq0G4B3CyeY6wYHN3yt9PW0XpSriVlkMXe40PTKnXrLnZ9+fkDaog==" crossorigin="anonymous" />
				</header>
			</div>
			<div class="pageheight">
				<body class="epbody">
					<h1>{location.state.eventTitle}</h1>
					<h3>{location.state.companyName}</h3>


					{DeclaredWinnerPopup &&
						<PopupAutoClose closePopup={setDeclaredWinnerPopup}
							content={<>
								<a>Winner has been declared.</a>
							</>}
						/>}

					{EliminatedPlayerPopup &&
						<PopupAutoClose closePopup={setEliminatedPlayerPopup}
							content={<>
								<a>Player Has been eliminated.</a>
							</>}
						/>}

					{DeclaredWinnerFailPopup &&
						<PopupAutoClose closePopup={setDeclaredWinnerFailPopup}
							content={<>
								<a>Error declaring winner, please try again.</a>
							</>}
						/>}

					{EliminatedPlayerFailPopup &&
						<PopupAutoClose closePopup={setEliminatedPlayerFailPopup}
							content={<>
								<a>Error eliminating player, please try agian.</a>
							</>}
						/>}
					{isEnd ? (
						<h2>The event has ended</h2>
					) : toshow}
				</body>
			</div>
			<div>
				<footer>
					<Footer />
				</footer>
			</div>
		</div>
	);
}

export default ManageEvent;