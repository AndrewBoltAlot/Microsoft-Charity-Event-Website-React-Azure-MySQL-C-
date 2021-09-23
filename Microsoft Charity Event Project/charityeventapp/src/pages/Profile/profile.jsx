import React, { useContext, useState, useEffect } from 'react'
import Header from '../../components/header/header'
import Footer from '../../components/footer/footer.jsx'
import { UserContext } from "../../UserContext";
import 'bootstrap/dist/css/bootstrap.min.css';
import InnerHTML from 'dangerously-set-html-content'
import styles from './profile.css'
import { useHistory } from "react-router-dom";
import Popup from '../../components/popup/popup';
import PopupAutoClose from '../../components/popup/popupAutoClose';


const Profile = (props) =>
{
	let history = useHistory();
	const { user5, setUser5, name, setName, verified, setVerified, tabOpen, setTabOpen, url, urlFrontEnd } = useContext(UserContext);

	const [userData, setUserData] = useState({});
	const [isParticipant, setisParticipant] = useState(false);
	const [events, setEvents] = useState([]);
	const [eventsMsg, setEventsMsg] = useState("");
	const [Password, setPassword] = useState("");
	const [NewPassword, setNewPassword] = useState("");
	const [repPassword, setRepPassword] = useState("");
	const [Email, setEmail] = useState("");
	const [toShowButton, setToShowButton] = useState(true);
	const [isOpen, setIsOpen] = useState(false);
	const [VerificationCode, setVerificationCode] = useState("");
	const [multipleAccounts, setmultipleAccounts] = useState(0);
	const [copiedClippboard, setcopiedClippboard] = useState(false);
	const [eventStarted, seteventStarted] = useState(false);
	const [eventCancelled, seteventCancelled] = useState(false);
	const [passwordChanged, setpasswordChanged] = useState(false);
	const [passwordChangedfail, setpasswordChangedfail] = useState(false);
	const [passwordsMatchFail, setpasswordsMatchFail] = useState(false);
	const [emailVerificationSuccessful, setemailVerificationSuccessful] = useState(false);
	const [emailVerificationFail, setemailVerificationFail] = useState(false);
	const [verificationCodeSent, setverificationCodeSent] = useState(false);
	const [verificationCodeSentfail, setverificationCodeSentfail] = useState(false);
	const [tooFewParticipants, settooFewParticipants] = useState(false);




	const [showOngoing, setshowOngoing] = useState(true);
	const [showWaiting, setshowWaiting] = useState(true);
	const [showCompleted, setshowCompleted] = useState(true);



	const copyClippboard = (inviteId) =>
	{
		const el = document.createElement("input");
		el.value = "" + urlFrontEnd + "/EventDetails?inviteLink=" + inviteId + "";
		document.body.appendChild(el);
		el.select();
		document.execCommand("copy");
		document.body.removeChild(el);
		setcopiedClippboard(true);
	}


	const clearPasswordFields = () =>
	{

		document.getElementById("pas1").value = "";
		document.getElementById("pas2").value = "";
		document.getElementById("pas3").value = "";
	}

	const dropdownopen = (e) =>
	{
		if (e === "showOngoing")
		{
			setshowOngoing(!showOngoing)
		} else if (e === "showWaiting")
		{
			setshowWaiting(!showWaiting)
		} else
		{
			setshowCompleted(!showCompleted);
		}
	}

	setTimeout(function ()
	{
		if (showOngoing)
		{
			document.getElementById("ongoingdropdown").setAttribute("class", "fas fa-angle-down");
		} else if (!showOngoing)
		{
			document.getElementById("ongoingdropdown").setAttribute("class", "fas fa-angle-right");
		}
		if (showWaiting)
		{
			document.getElementById("waitingdropdown").setAttribute("class", "fas fa-angle-down");
		} else if (!showWaiting)
		{
			document.getElementById("waitingdropdown").setAttribute("class", "fas fa-angle-right");
		}
		if (showCompleted)
		{
			document.getElementById("completeddropdown").setAttribute("class", "fas fa-angle-down");
		} else if (!showCompleted)
		{
			document.getElementById("completeddropdown").setAttribute("class", "fas fa-angle-right");
		}

	}, 150);



	window.onload = function ()
	{


		document.getElementById("general").setAttribute("class", "list-group-item list-group-item-action");
		document.getElementById("account-general").setAttribute("class", "tab-pane fade");

		setTimeout(function ()
		{
			if (user5 === "Organiser")
			{
				document.getElementById("accountTypeCB").checked = true;
			} else
			{
				document.getElementById("accountTypeCB").checked = false;
			}
		}, 300);



		switch (tabOpen)
		{
			case "password":
				document.getElementById("password").setAttribute("class", "list-group-item list-group-item-action active");
				document.getElementById("account-change-password").setAttribute("class", "tab-pane fade active show");
				setToShowButton(false);
				break;
			case "organised":
				document.getElementById("organised").setAttribute("class", "list-group-item list-group-item-action active");
				document.getElementById("account-events").setAttribute("class", "tab-pane fade active show");
				getEvents("organised");
				setToShowButton(true);
				break;
			case "registered":
				document.getElementById("registered").setAttribute("class", "list-group-item list-group-item-action active");
				document.getElementById("account-events").setAttribute("class", "tab-pane fade active show");
				getEvents("registered");
				setToShowButton(true);
				break;
			default:
				document.getElementById("general").setAttribute("class", "list-group-item list-group-item-action active");
				document.getElementById("account-general").setAttribute("class", "tab-pane fade active show");
				setToShowButton(true);
		}

	};




	const getEvents = async (tab) =>
	{
		setToShowButton(true);
		if (user5 === "Participant")
		{
			setisParticipant(true);
			const response = await fetch("" + url + "/api/ParticipantEvents/" + name + "", {
				method: "GET",
				header: { "Content-Type": "application/json" },
				Accept: 'application/json'
			});
			if (response.ok)
			{
				setEvents(await response.json());
				setEventsMsg("");
			}
			else
			{
			}
		} else if (user5 === "Organiser")
		{
			const response = await fetch("" + url + "/api/OrganisersEvents/" + name + "", {
				method: "GET",
				header: { "Content-Type": "application/json" },
				Accept: 'application/json'
			});
			if (response.ok)
			{
				setEvents(await response.json());
			}
		}

		setTabOpen(tab);

	}

	const changePassword = async () =>
	{
		const passwordDetails = {
			Email,
			Password,
			NewPassword
		};
		if (NewPassword === repPassword)
		{
			const response = await fetch("" + url + "/api/resetPassword", {
				method: "POST",
				headers: { "Content-Type": "application/json; odata=verbose" },
				Accept: 'application/json',
				body: JSON.stringify(passwordDetails)

			}).then(async response =>
			{
				if (!response.ok)
				{
					setpasswordChangedfail(true);
				}
				else
				{
					setpasswordChanged(true);
					document.getElementById("pas1").value = "";
					document.getElementById("pas2").value = "";
					document.getElementById("pas3").value = "";
				}
			}).catch(error =>
			{
				console.error("Error :", error);
			})
		}
		else
		{
			setpasswordsMatchFail(true);
		}
	}



	const startEvent = (eventId) =>
	{

		fetch("" + url + "/api/startEvent/" + eventId + "", {
			method: "POST",
			headers: { "Content-Type": "application/json" },
			credentials: "include",
		}).then(res =>
		{
			if (!res.ok)
			{

			}
			else
			{
				seteventStarted(true);
				window.location.reload(true);
			}
		}).catch(err =>
		{

		});

	}

	const onTabChange = (tab) =>
	{
		if (tab === "password")
		{
			setToShowButton(false)
		} else
		{
			setToShowButton(true)
		}

		setTabOpen(tab);
	}

	const changeAccount = () => 
	{

		if (user5 === "Organiser")
		{
			setUser5("Participant");
		} else
		{
			setUser5("Organiser");
		}

		window.location.reload(true);
	}


	const resendVerificationCode = async () =>
	{
		if (user5 === "Participant")
		{
			const response = await fetch("" + url + "/api/PsendEmailVerification/" + name + "", {
				method: "GET",
				header: { "Content-Type": "application/json" },
				Accept: 'application/json'
			});
			if (response.ok)
			{
				setverificationCodeSent(true);
			}
			else
			{
				setverificationCodeSentfail(true);
			}
		} else if (user5 === "Organiser")
		{
			const response = await fetch("" + url + "/api/OsendEmailVerification/" + name + "", {
				method: "GET",
				header: { "Content-Type": "application/json" },
				Accept: 'application/json'
			});
			if (response.ok)
			{
				setverificationCodeSent(true);
			}
			else
			{
				setverificationCodeSentfail(true);
			}
		}

	}
	const verifyCode = async () =>
	{
		const verificationDetails = {
			Email,
			VerificationCode
		};
		if (user5 === "Participant")
		{
			const response = await fetch("" + url + "/api/PverifyEmail", {
				method: "POST",
				headers: { "Content-Type": "application/json; odata=verbose" },
				Accept: 'application/json',
				body: JSON.stringify(verificationDetails)
			});
			if (response.ok)
			{
				setVerified(true);
				setIsOpen(false);
				setemailVerificationSuccessful(true)
				window.location.reload(true);
			}
			else
			{
				setemailVerificationFail(true);
			}

		}
		else if (user5 === "Organiser")
		{
			const response = await fetch("" + url + "/api/OverifyEmail", {
				method: "POST",
				headers: { "Content-Type": "application/json; odata=verbose" },
				Accept: 'application/json',
				body: JSON.stringify(verificationDetails)
			});
			if (response.ok)
			{
				setVerified(true);
				setIsOpen(false);
				setemailVerificationSuccessful(true)
			}
			else
			{
				setemailVerificationFail(true);
			}
		}
	}

	const cancelEvent = (eventId) =>
	{
		fetch("" + url + "/api/cancelevent/" + eventId + "", {
			method: "POST",
			headers: { "Content-Type": "application/json" },
			credentials: "include",
		}).then(res =>
		{
			if (!res.ok)
			{
				seteventCancelled(true);
			}
			else
			{
				window.location.reload(true);
			}
		}).catch(err =>
		{

		});
	}

	useEffect(() =>
	{

		setTimeout(function ()
		{
			if (user5 === "Organiser")
			{
				document.getElementById("accountTypeCB").checked = true;
			} else
			{
				document.getElementById("accountTypeCB").checked = false;
			}
		}, 300);

		(
			async () =>
			{
				if (user5 === "Participant")
				{
					setisParticipant(true);
					const response = await fetch("" + url + "/api/ParticipantsDetails/" + name + "", {
						method: "GET",
						header: { "Content-Type": "application/json" },
						Accept: 'application/json'
					});
					if (response.ok)
					{
						setUserData(await response.json());
						setEmail(name);
					}
					else
					{

					}
				} else if (user5 === "Organiser")
				{
					const response = await fetch("" + url + "/api/OrganisersDetails/" + name + "", {
						method: "GET",
						header: { "Content-Type": "application/json" },
						Accept: 'application/json'
					});
					if (response.ok)
					{
						setUserData(await response.json());
						setEmail(name);
					}
					else
					{

					}
				}

			}

		)();
		(
			async () =>
			{
				const response = await fetch("" + url + "/api/getNumberOfUserAccounts/" + name + "", {
					method: "GET",
					header: { "Content-Type": "application/json" },
					credentials: "include"
				});

				if (response.ok)
				{
					const list = await response.json();
					setmultipleAccounts(list);

				}
			}
		)();
	}, [name, verified]);


	const html = `
    
    <script src="https://code.jquery.com/jquery-1.10.2.min.js"></script>
	<script src="https://cdn.jsdelivr.net/npm/bootstrap@4.5.0/dist/js/bootstrap.bundle.min.js"></script>
  `


	function capitalizeFirstLetter(string)
	{
		if (string != undefined)
		{
			return string.charAt(0).toUpperCase() + string.slice(1);
		}
	}


	return (
		<div>
			<div>
				<header>
					<Header />
					<InnerHTML html={html} />
					<link href="https://cdn.jsdelivr.net/npm/bootstrap@4.5.0/dist/css/bootstrap.min.css" rel="stylesheet" /> </header>
			</div>
			<div class="pageheight">
				<div class="pbody">
					<div class="container light-style flex-grow-1 container-p-y">
						<h4 class="font-weight-bold py-3 mb-4">Profile
						</h4>
						<div class="card overflow-hidden">
							<div class="consistent-height row no-gutters row-bordered row-border-light">
								<div class="col-md-3 pt-0">
									<div class="list-group list-group-flush account-settings-links" id="myList">
										<a class="list-group-item list-group-item-action active" data-toggle="list" href="#account-general" id="general" onClick={() => onTabChange("general")}>General</a>
										<a class="list-group-item list-group-item-action" data-toggle="list" href="#account-change-password" id="password" onClick={() => onTabChange("password")}>Change password</a>
										{isParticipant ? <a class="list-group-item list-group-item-action" data-toggle="list" href="#account-events" id="registered" onClick={() => getEvents("registered")}>Registered Events</a> : null}
										{!isParticipant ? <a class="list-group-item list-group-item-action" data-toggle="list" href="#account-events" id="organised" onClick={() => getEvents("organised")}>Organized Events</a> : null}
									</div>
								</div>
								<div class="col-md-9">
									<div class="tab-content">
										<div class="tab-pane fade active show" id="account-general">
											<div class="card-body media align-items-center"> <img src="https://bootdey.com/img/Content/avatar/avatar1.png" alt="" class="d-block ui-w-80" />
												<div class="media-body ml-4">
													{(multipleAccounts === 1) ?
														<div>
															<div class="addSpace"> Participant  &nbsp;
																<label class="switch">
																	<input type="checkbox" id="accountTypeCB" onChange={() => changeAccount()} />
																	<span class="slider round"></span>
																</label>  &nbsp; Organiser
															</div>
														</div> :
														<input type="hidden" id="accountTypeCB" />}

													<div class="addSpace" />

												</div>
											</div>
											<hr class="border-light m-0" />
											<div class="card-body">

												{isParticipant ?
													<div class="form-group">

														<label class="form-label"><strong>Name</strong></label><br />
														<label class="form-label">{capitalizeFirstLetter(userData.firstName)}</label>
														<hr class="border-light m-0" />
													</div> : null}
												{!isParticipant ?
													<div class="form-group">
														<label class="form-label"><strong>Company</strong></label><br />
														<label class="form-label mb-1">{capitalizeFirstLetter(userData.companyName)}</label>
														<hr class="border-light m-0" /></div> : null}

												<div class="form-group">
													<label class="form-label"><strong>E-mail</strong></label><br />
													<label class="form-label mb-1">{capitalizeFirstLetter(userData.email)}</label>
													<hr class="border-light m-0" />
													{!verified ? <div class="alert alert-warning mt-3"> Your account is not verified. Please click on the below link to verify your account.
														<br /> <a href="javascript:void(0)" onClick={() => setIsOpen(true)} >Verify Account</a>
													</div> : <a> </a>}
												</div>
												<div class="form-group">
													<label class="form-label"><strong>Contact Number</strong></label><br />
													<label class="form-label mb-1">{userData.phoneNumber}</label>
													<hr class="border-light m-0" /> </div>
												<div class="form-group">
													<label class="form-label"><strong>Address</strong></label><br />
													<label class="form-label">{capitalizeFirstLetter(userData.address)}</label>
													<hr class="border-light m-0" />
												</div>
												<div style={{ display: "inline-block" }}>
													{isOpen && <Popup closePopup={setIsOpen}
														content={<>
															<input type="text" class="form-control" placeholder="Enter your verification code" required onChange={e => setVerificationCode(e.target.value)} />
															<button type="button" class="button " onClick={() => verifyCode()}>Verify</button>
															<button type="button" class="button " onClick={() => resendVerificationCode()}>Resend Code</button>
														</>}
													/>}
												</div>
											</div>
										</div>
										<div class="tab-pane fade" id="account-change-password">
											<div class="card-body pb-2">
												<div class="form-group">
													<label class="form-label"><strong>Current password</strong></label>
													<input type="password" class="form-control"
														id="pas1"
														pattern="^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$"
														title="Minimum eight characters, at least one letter, one number and one special character."
														required onChange={e => setPassword(e.target.value)} /> </div>
												<div class="form-group">
													<label class="form-label"><strong>New password</strong></label>
													<input type="password" class="form-control"
														id="pas2"
														pattern="^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$"
														title="Minimum eight characters, at least one letter, one number and one special character."
														required onChange={e => setNewPassword(e.target.value)} /> </div>
												<div class="form-group">
													<label class="form-label"><strong>Repeat new password</strong></label>
													<input type="password" class="form-control"
														id="pas3"
														pattern="^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$"
														title="Minimum eight characters, at least one letter, one number and one special character."
														required onChange={e => setRepPassword(e.target.value)} /> </div>
											</div>

										</div>
										<div class="tab-pane fade" id="account-events">
											<div class="card-body pb-2">
												{eventsMsg}

												<div id="accordion" class="accordion">
													<div class="eventsDropdown" onClick={() => dropdownopen("showWaiting")}>
														<i id="waitingdropdown" class="fas fa-angle-down"></i>&nbsp; &nbsp;
														<label class="event-status-title"><strong> Waiting to start </strong></label>
														<div id="headingboxgray"> <div class="card"></div> </div></div>

													{events.map(data =>

														!data.competitionStarted && showWaiting ?

															<div class="card">
																<div class="card-header" id="headingOne">
																	<h5 class="mb-0">
																		<div class="usersEvents">
																			<button class="event-butt" data-toggle="collapse" data-target="#collapseOne" aria-expanded="true" aria-controls="collapseOne"
																				onClick={() =>
																				{
																					history.push({
																						pathname: './ManageEvents',
																						state: { eventId: data.eventId, eventTitle: data.title, orgname: data.email, compStarted: data.competitionStarted }
																					});
																				}}>
																				{capitalizeFirstLetter(data.title)}

																			</button>

																			<div class="buttonProfile">
																				<button class="invite-butt"
																					onClick={() =>
																					{
																						copyClippboard(data.invite_id)

																					}}>
																					Copy Invite link

																				</button>

																				<span> </span>
																				{!data.competitionStarted && !isParticipant ? (<button type="button" class="invite-butt"
																					onClick={() =>
																					{

																						if (data.numberOfParticipants > 1)
																						{
																							startEvent(data.eventId);
																							seteventStarted(true);

																						}
																						else
																						{
																							settooFewParticipants(true);
																						}
																					}}>
																					Start Event
																				</button>) : null}

																				<span> </span>
																				{!data.competitionStarted && !isParticipant ? (<button type="button" id="cancel-event" class="invite-butt"
																					onClick={() =>
																					{
																						cancelEvent(data.eventId);
																						seteventCancelled(true);
																					}}>
																					Cancel
																				</button>) : null}
																			</div>
																		</div>
																	</h5>
																</div>
															</div>

															: <a></a>
													)}
													<div class="eventsDropdown" onClick={() => dropdownopen("showOngoing")}>
														<i id="ongoingdropdown" class="fas fa-angle-down"></i>&nbsp; &nbsp;
														<label class="event-status-title" style={{ paddingTop: "10px" }}><strong>Ongoing</strong></label>
														<div id="headingboxgray"> <div class="card"></div> </div></div>

													{events.map(data =>

														data.competitionStarted && !data.competitionCompleted && showOngoing ?

															<div class="card">
																<div class="card-header" id="headingOne">
																	<h5 class="mb-0">
																		<div class="usersEvents">
																			<button class="event-butt" data-toggle="collapse" data-target="#collapseOne" aria-expanded="true" aria-controls="collapseOne"
																				onClick={() =>
																				{
																					history.push({
																						pathname: './ManageEvents',
																						state: { eventId: data.eventId, eventTitle: data.title, orgname: data.email, compStarted: data.competitionStarted }
																					});
																				}}>
																				{capitalizeFirstLetter(data.title)}

																			</button>

																			<div class="buttonProfile">
																				<button type="button" class="invite-butt"
																					onClick={() =>
																					{
																						copyClippboard(data.invite_id)
																					}}>
																					Copy Invite link

																				</button>

																				<span> </span>
																				{!data.competitionStarted && !isParticipant ? (<button type="button" class="invite-butt"
																					onClick={() =>
																					{
																						startEvent(data.eventId);
																						seteventStarted(true);
																					}}>
																					Start Event
																				</button>) : null}

																				<span> </span>
																				{!data.competitionStarted && !isParticipant ? (<button type="button" class="invite-butt"
																					onClick={() =>
																					{
																						cancelEvent(data.eventId);
																						seteventCancelled(true);
																					}}>
																					Cancel
																				</button>) : null}
																			</div>
																		</div>
																	</h5>
																</div>
															</div>

															: <a></a>
													)}

													<div class="eventsDropdown" onClick={() => dropdownopen("showCompleted")}>
														<i id="completeddropdown" class="fas fa-angle-down"></i>&nbsp; &nbsp;
														<label class="event-status-title" style={{ paddingTop: "10px" }}><strong> Completed </strong></label>
														<div id="headingboxgray"> <div class="card"></div> </div></div>
													{events.map(data =>

														data.competitionStarted && data.competitionCompleted && showCompleted ?

															<div class="card">
																<div class="card-header" id="headingOne">
																	<h5 class="mb-0">
																		<div class="usersEvents">
																			<button class="event-butt" data-toggle="collapse" data-target="#collapseOne" aria-expanded="true" aria-controls="collapseOne"
																				onClick={() =>
																				{
																					history.push({
																						pathname: './ManageEvents',
																						state: { eventId: data.eventId, eventTitle: data.title, orgname: data.email, compStarted: data.competitionStarted }
																					});
																				}}>
																				{capitalizeFirstLetter(data.title)}

																			</button>

																			<div class="buttonProfile">
																				<button type="button" class="invite-butt"
																					onClick={() =>
																					{
																						copyClippboard(data.invite_id)
																					}}>
																					Copy Invite link

																				</button>

																				<span> </span>
																				{!data.competitionStarted && !isParticipant ? (<button type="button" class="invite-butt"
																					onClick={() =>
																					{
																						startEvent(data.eventId);
																						seteventStarted(true);
																					}}>
																					Start Event
																				</button>) : null}

																				<span> </span>
																				{!data.competitionStarted && !isParticipant ? (<button type="button" class="invite-butt"
																					onClick={() =>
																					{
																						cancelEvent(data.eventId);
																						seteventCancelled(true);
																					}}>
																					Cancel
																				</button>) : null}
																			</div>
																		</div>
																	</h5>
																</div>
															</div>

															: <a></a>
													)}

												</div>
											</div>

										</div>
									</div>
								</div>
							</div>
						</div>



						{verificationCodeSent &&
							<PopupAutoClose closePopup={setverificationCodeSent}
								content={<>
									<a>Verification code sent. Please check your email.</a>
								</>}
							/>}

						{verificationCodeSentfail &&
							<PopupAutoClose closePopup={setverificationCodeSentfail}
								content={<>
									<a>Error sending verrification code. Please try again.</a>
								</>}
							/>}

						{emailVerificationSuccessful &&
							<PopupAutoClose closePopup={setemailVerificationSuccessful}
								content={<>
									<a>Email verifed.</a>
								</>}
							/>}

						{emailVerificationFail &&
							<PopupAutoClose closePopup={setemailVerificationFail}
								content={<>
									<a>Incorrect verification code.</a>
								</>}
							/>}


						{copiedClippboard &&
							<PopupAutoClose closePopup={setcopiedClippboard}
								content={<>
									<a>Copied to clipboard.</a>
								</>}
							/>}

						{eventStarted &&
							<PopupAutoClose closePopup={seteventStarted}
								content={<>
									<a>Event successfully started.</a>
								</>}
							/>}

						{eventCancelled &&
							<PopupAutoClose closePopup={seteventCancelled}
								content={<>
									<a>Event successfully cancelled.</a>
								</>}
							/>}

						{passwordChanged &&
							<PopupAutoClose closePopup={setpasswordChanged}
								content={<>
									<a>Password successfully changed.</a>
								</>}
							/>}

						{passwordChangedfail &&
							<PopupAutoClose closePopup={setpasswordChangedfail}
								content={<>
									<a>Inncorrect current password entered.</a>
								</>}
							/>}

						{passwordsMatchFail &&
							<PopupAutoClose closePopup={setpasswordsMatchFail}
								content={<>
									<a>New password does not match.</a>
								</>}
							/>}

						{tooFewParticipants &&
							<PopupAutoClose closePopup={settooFewParticipants}
								content={<>
									<a>Error starting event! - There must be at least 2 participants in the event.</a>
								</>}
							/>}

						{!toShowButton ? <div class="text-right mt-3">
							<button type="button" class="button " onClick={() => changePassword()}>Change Password</button>&nbsp;
							<button type="button" class="button " onClick={() => clearPasswordFields()} >Cancel</button>
						</div> : null}
						<br /> </div>


				</div>
			</div>
			<div>
				<footer>
					<Footer />
				</footer>
			</div>
		</div>
	)
}

export default Profile;