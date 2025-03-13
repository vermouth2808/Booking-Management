import { BrowserRouter as Router, Routes, Route,Navigate } from "react-router-dom";
import LoginPage from "./pages/LoginPage";
import HomePage from "./pages/HomePage";
import DetailMovie from "./pages/detailMovie/DetailMovie"
import ProtectedRoute from "./components/ProtectedRoute";
import MainLayout from "./components/layout/MainLayout"

const App = () => {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<Navigate to="/home" />} />

        <Route path="/login" element={<LoginPage />} />

        <Route path="/home" element={
          <ProtectedRoute>
            <MainLayout>
              <HomePage />
            </MainLayout>
          </ProtectedRoute>
        } />

        <Route path="/movie/:id" element={
          <ProtectedRoute>
            <MainLayout>
              <DetailMovie />
            </MainLayout>
          </ProtectedRoute>
        } />
      </Routes>
    </Router>
  );
};

export default App;
