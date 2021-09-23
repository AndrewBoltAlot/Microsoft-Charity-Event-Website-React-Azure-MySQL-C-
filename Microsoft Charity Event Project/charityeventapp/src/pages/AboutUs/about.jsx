import React from 'react';
import './About.css';
import Header from '../../components/header/header'
import Footer from '../../components/footer/footer.jsx'
import Profile from './Profile.jpeg'
import logo from './logo.png'
import sports from './sports.jpg'
import p3 from './p3.jpeg'

const about = (props) =>
{

  return (
    <div>
      <div>
        <header>
          <Header name={props.name} setName={props.setName} />
        </header>
      </div>
      <div>
        <body>
          <div>
            <div class="cards-list">
              <div className="AboutCard">
                <div className="card_img"> <img src={logo} /> </div>
                {/* <img src={Profile} alt="Andrew" style="width:100%"> */}
                <h1>Andrew Bolt</h1>
                <h4 className="title">Developer</h4>
                <h5>University College Dublin</h5>
                <h5>Andrew.bolt@ucdconnect.ie</h5>
                <div className="About_social">
                  <a href="https://www.facebook.com/"><i class="fab fa-facebook-f"></i></a>
                  <a> </a>
                  <a href="https://twitter.com/"><i class="fab fa-twitter"></i></a>
                  <a> </a>
                  <a href="https://www.linkedin.com/"><i class="fab fa-linkedin"></i></a>
                  <a>  </a>
                  <a href="https://www.instagram.com/accounts/login/"><i class="fab fa-instagram"></i></a>
                  <a> </a>
                </div>
                {/* <p><button>Contact</button></p> */}
              </div>
              <div className="AboutCard 1">
                <div class="card_img"> <img src={logo} /> </div>
                {/* <img src="img.jpg" alt="John" style="width:100%"> */}
                <h1>Rakshanda Chavan</h1>
                <h4 className="title">Developer</h4>
                <h5>University College Dublin</h5>
                <h5>Rakshanda.chavan@ucdconnect.ie</h5>
                <div className="About_social">
                  <a href="https://www.facebook.com/"><i class="fab fa-facebook-f"></i></a>
                  <a> </a>
                  <a href="https://twitter.com/"><i class="fab fa-twitter"></i></a>
                  <a> </a>
                  <a href="https://www.linkedin.com/"><i class="fab fa-linkedin"></i></a>
                  <a>  </a>
                  <a href="https://www.instagram.com/accounts/login/"><i class="fab fa-instagram"></i></a>
                  <a> </a>
                </div>
              </div>

              <div className="AboutCard 3">
                <div class="card_img"> <img src={logo} /> </div>
                {/* <img src="img.jpg" alt="John" style="width:100%"> */}
                <h1>Shubham Narandekar</h1>
                <h4 className="title">Developer</h4>
                <h5>University College Dublin</h5>
                <h5>Shubham.narandekar@ucdconnect.ie</h5>
                <div className="About_social">
                  <a href="https://www.facebook.com/"><i class="fab fa-facebook-f"></i></a>
                  <a> </a>
                  <a href="https://twitter.com/"><i class="fab fa-twitter"></i></a>
                  <a> </a>
                  <a href="https://www.linkedin.com/"><i class="fab fa-linkedin"></i></a>
                  <a>  </a>
                  <a href="https://www.instagram.com/accounts/login/"><i class="fab fa-instagram"></i></a>
                  <a> </a>
                </div>
              </div>

              <div className="AboutCard 2">
                <div class="card_img"> <img src={logo} /> </div>
                {/* <img src="img.jpg" alt="John" style="width:100%"> */}
                <h1>Anurag Thoke</h1>
                <h4 className="title">Developer</h4>
                <h5>University College Dublin</h5>
                <h5>EAnurag.thoke@ucdconnect.ie</h5>
                <div className="About_social">
                  <a href="https://www.facebook.com/"><i class="fab fa-facebook-f"></i></a>
                  <a> </a>
                  <a href="https://twitter.com/"><i class="fab fa-twitter"></i></a>
                  <a> </a>
                  <a href="https://www.linkedin.com/"><i class="fab fa-linkedin"></i></a>
                  <a>  </a>
                  <a href="https://www.instagram.com/accounts/login/"><i class="fab fa-instagram"></i></a>
                  <a> </a>
                </div>
              </div>

              <div className="AboutCard 4">
                <div class="card_img"> <img src={logo} /> </div>
                {/* <img src="img.jpg" alt="John" style="width:100%"> */}
                <h1>Nikola Zlokapa</h1>
                <h4 className="title">Developer</h4>
                <h5>University College Dublin</h5>
                <h5>Nikola.zlokapa@ucdconnect.ie</h5>
                <div className="About_social">
                  <a href="https://www.facebook.com/"><i class="fab fa-facebook-f"></i></a>
                  <a> </a>
                  <a href="https://twitter.com/"><i class="fab fa-twitter"></i></a>
                  <a> </a>
                  <a href="https://www.linkedin.com/"><i class="fab fa-linkedin"></i></a>
                  <a>  </a>
                  <a href="https://www.instagram.com/accounts/login/"><i class="fab fa-instagram"></i></a>
                  <a> </a>
                </div>
              </div>
              <div className="AboutCard 5">
                <div class="card_img"> <img src={logo} /> </div>
                {/* <img src="img.jpg" alt="John" style="width:100%"> */}
                <h1>Shaoshu Zhu</h1>
                <h4 className="title">Developer</h4>
                <h5>University College Dublin</h5>
                <h5>Shaoshu.zhu@ucdconnect.ie</h5>
                <div className="About_social">
                  <a href="https://www.facebook.com/"><i class="fab fa-facebook-f"></i></a>
                  <a> </a>
                  <a href="https://twitter.com/"><i class="fab fa-twitter"></i></a>
                  <a> </a>
                  <a href="https://www.linkedin.com/"><i class="fab fa-linkedin"></i></a>
                  <a>  </a>
                  <a href="https://www.instagram.com/accounts/login/"><i class="fab fa-instagram"></i></a>
                  <a> </a>
                </div>
              </div>
              <div className="AboutCard 6">
                <div class="card_img"> <img src={logo} /> </div>
                {/* <img src="img.jpg" alt="John" style="width:100%"> */}
                <h1>Xinhao Chen</h1>
                <h4 className="title">Developer</h4>
                <h5>University College Dublin</h5>
                <h5>Xinhao.chen@ucdconnect.ie</h5>
                <div className="About_social">
                  <a href="https://www.facebook.com/"><i class="fab fa-facebook-f"></i></a>
                  <a> </a>
                  <a href="https://twitter.com/"><i class="fab fa-twitter"></i></a>
                  <a> </a>
                  <a href="https://www.linkedin.com/"><i class="fab fa-linkedin"></i></a>
                  <a>  </a>
                  <a href="https://www.instagram.com/accounts/login/"><i class="fab fa-instagram"></i></a>
                  <a> </a>
                </div>
              </div>
            </div>
          </div>
        </body>
      </div>
      <div>
        <footer>
          <Footer /> </footer>
      </div>
    </div>
  )
}

export default about

