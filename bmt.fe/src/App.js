import logo from './logo.svg';
import './App.css';
import {Button,Checkbox,Form} from 'antd';
import React from 'react';


function App() {
  const name ="tuan anh"
  return (
    <div className="App">
      <Button>Submit</Button>
      
    <h1>{name}</h1>
    <mycomponent></mycomponent>
    
    </div>
    
  );
}

export default App;
