import React from "react";
import { useLocation } from "react-router-dom";
import Header from "./Header";
import Footer from "./Footer";

const MainLayout = ({ children }) => {
    const location = useLocation();
    const isLoginPage = location.pathname.includes("/login"); 

    return (
        <>
            {!isLoginPage && <Header />}
            <main style={{ minHeight: "calc(100vh - 100px)", paddingTop: "60px" }}>
                {children}
            </main>
            {!isLoginPage && <Footer />}
        </>
    );
};

export default MainLayout;
