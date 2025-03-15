import React from "react";
import Footer from "./Footer";
import Header from "./Header";


const MainLayout = ({ children }) => {
  return (
    <div className="layout-wrapper">
      <Header />
      <main className="main-layout">{children}</main>
      <Footer />
    </div>
  );
};



export default MainLayout;
