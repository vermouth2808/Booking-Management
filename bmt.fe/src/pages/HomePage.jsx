import React, { useEffect, useState } from "react";
import api from "../api/api";
import { Layout, Switch, Card, Typography, Button } from "antd";
import { MoonOutlined, SunOutlined } from "@ant-design/icons";
import "antd/dist/reset.css";
import { motion } from "framer-motion";
import MovieService from "../services/MovieService";

const { Header, Content, Footer } = Layout;
const { Title, Text } = Typography;

const HomePage = () => {
  const [movie, setMovie] = useState([]);
  const [darkMode, setDarkMode] = useState(false);

  useEffect(() => {
    const fetchMovies = async () => {
      try {
        const movies = await MovieService.searchMovies();
        console.log("Data movies:", movies);
  
        if (Array.isArray(movies)) {
          setMovie(movies); 
        } else {
          setMovie([]); 
        }
      } catch (error) {
        console.error("Error fetching movies:", error);
        setMovie([]); 
      }
    };
  
    fetchMovies();
  }, []);
  

  return (
    <Layout style={{ minHeight: "100vh", background: darkMode ? "#121212" : "#f5f5f5" }}>
      <Header style={{
        display: "flex", justifyContent: "space-between", alignItems: "center",
        background: darkMode ? "#1e1e1e" : "#ffffff", padding: "0 20px",
        borderBottom: darkMode ? "1px solid #444" : "1px solid #ddd"
      }}>
        <Title level={3} style={{ color: darkMode ? "#fff" : "#000" }}>CineFuture</Title>
        <Switch
          checked={darkMode}
          checkedChildren={<MoonOutlined />}
          unCheckedChildren={<SunOutlined />}
          onChange={() => setDarkMode(!darkMode)}
        />
      </Header>

      <Content style={{ padding: "20px", display: "flex", flexWrap: "wrap", justifyContent: "center", gap: "20px" }}>
        {movie?.map((movie, index) => (
          <motion.div
            key={movie.movieId || index}
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.5, delay: index * 0.1 }}
          >
            <Card
              hoverable
              style={{ width: 240, borderRadius: "10px", overflow: "hidden", background: darkMode ? "#242424" : "#fff" }}
              cover={<img alt="movie poster" src={movie.posterUrl || `${movie.title}`} />}
            >
              <Title level={4} style={{ color: darkMode ? "#fff" : "#000" }}>{movie.title}</Title>
              <Text style={{ color: darkMode ? "#bbb" : "#333" }}>Genre: {movie.genre}</Text>
              <div style={{ marginTop: "10px", textAlign: "center" }}>
                <Button type="primary" shape="round">Đặt ngay</Button>
              </div>
            </Card>
          </motion.div>
        ))}
      </Content>

      <Footer style={{ textAlign: "center", background: darkMode ? "#1e1e1e" : "#ffffff", color: darkMode ? "#bbb" : "#333" }}>
        CineFuture ©2025 - Experience the Future of Cinema
      </Footer>
    </Layout>
  );
}

export default HomePage;
