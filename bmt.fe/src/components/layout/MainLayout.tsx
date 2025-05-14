import React, { FC } from "react";
import Footer from "./Footer";
import Header from "./Header";

const MainLayout: FC<{ children: React.ReactNode }> = ({ children }) => {
  return (
    <div className="layout-wrapper">
      <Header />
      <main className="main-layout">{children}</main>
      <Footer />
    </div>
  );
};

export default MainLayout;
