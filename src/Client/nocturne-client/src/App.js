import logo from './logo.svg';
import './App.css';
import MessageItem from './components/message/MessageItem.jsx'

function App() {

  var message = {content:"11dasdasdasdasdasd322", dateTime:"Today", from: "DROddddddddddddddddddddddddP Table"}

  return (
    <div className="App">
        <MessageItem message={message}/>
    </div>
  );
}

export default App;
