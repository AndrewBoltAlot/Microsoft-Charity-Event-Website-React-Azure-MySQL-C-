import firebase from 'firebase';

const firebaseConfig = {
    apiKey: "AIzaSyBlVzCyEgzSsYFyBGsKrnVw5uToe6HZMtA",
    authDomain: "charityevent-831c9.firebaseapp.com",
    projectId: "charityevent-831c9",
    storageBucket: "charityevent-831c9.appspot.com",
    messagingSenderId: "731056268517",
    appId: "1:731056268517:web:2ffbc6a8beffcb2a2e600c",
    measurementId: "G-WY03XQ9XSH"
}

const fireApp = firebase.initializeApp(firebaseConfig);
console.log(fireApp);
var storage = firebase.storage();
export default storage