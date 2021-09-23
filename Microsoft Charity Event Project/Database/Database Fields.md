# Database Fields

This doc is for describing all fields in tables we would use in Charity Event project.



## Authentication

`Email`: Primary Key, it stores the user's(participant or organiser) email address.

`Password`: It stores the a hashed copy of the users password.

`Login_date`: It stores the time of the latest login. The format is `YYYY-MM-DD hh:mm:ss`.

`Salt`: It stores a unique salt for hashing the users password.

`Failed_attempts`: It stores the number of failed login attempts since the user last logged in.



## Participant

`Email`: Primary Key, it stores the participant's email address.

`First_name`: It stores the first name of participant.

`Last_name`: It stores the last name of participant.

`Phone_number`: It stores the participant's phone number.

`DOB`: It stores the birthday of participants. The format is`YYYY-MM-DD` .

`Address`: It stores the address of participants.

`Zip`: It stores the region code(like Eircode, Zip code) of the address of participants.

`Created_date`: It stores the time that participants register their account. The format is `YYYY-MM-DD hh:mm:ss`.



## Organiser

`Email`: Primary Key, it stores the organiser's email address.

`Company_Name`: It stores the name of organiser's company.

`Company_description`: It stores the description of company.

`Phone_number`: It stores the phone number of the contact in company.

`Address`: It stores the address of company.

`Zip`: It stores the region code(like Eircode, Zip code) of the address of company.

`Created_date`:It stores the time that organisers register their account. The format is `YYYY-MM-DD hh:mm:ss`.



## Event

`Event_id`: Primary Key, it stores the identifier of events.

`Organiser_email`: Foreign Key, it stores the organiser's email address.

`Title`: It stores the title of events.

`Type`: It stores the type of events.

`Created_date`: It stores the time that organisers register this event. The format is `YYYY-MM-DD hh:mm:ss`.

`Cost`: It stores cost of the ticket at one moment.

`registration_begin`: It stores the beginning time of registration for participants. The format is `YYYY-MM-DD hh:mm:ss`.

`registration_end`:It stores the end time of registration for participants. The format is `YYYY-MM-DD hh:mm:ss`.

`Privacy`: It stores the boolean value that shows if this event is public.

`IBAN`: It stores the IBAN that the company uses to receive payment from participants. 

`Image_path`: It stores the path of event image.

`Description`: It stores the description of this event.

`MaxNumberOfParticipants`: The maximum number of participants that can enter the competition.

`PayoutSplitPercentage`: The percentage of the total prize pool that will go to the competition winner.

`CompetitionStarted`: A boolean value to keep track if the competition has started.

## Event_selection

`Event_id`: The Id of the specific event.

`Selectino`: The available selection for the specific event.



## Event_location

`Location_id`: Primary Key, it stores the identifier of specific location.

`Address`: It stores the address in which this event is holding.

`Zip`:  It stores the region code(like Eircode, Zip code) of the address.

`Longitude`: It stores the longitude of the address.

`Latitude`:  It stores the latitude of the address.



## Ticket 

`Ticket_id`: Primary Key, it keeps track of the id for a particular ticket. 

`Participant_email`: It stores the email address of the participant that bought the ticket.  

`Price`: Stores the price for a particular ticket. 



##  Ticket_event

`Event_id`: Primary Key, it References the `Event_id` from the `Event` Table. 

`Ticket_id`: Primary Key, it References the `Ticket_id` from the `Ticket` Table. 

`Time_attended`: It stores the date/time a ticket has been scanned into an event in the format `YYYY-MM-DD hh:mm:ss `.

`Position`: The position the participant has finished in.

`Prize`: The prize the participant has won.

`Selection`: Participants selection for the event.

`Eliminated`: A boolean value to keep track if the player is still in the competition.

