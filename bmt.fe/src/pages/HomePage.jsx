import React, { useEffect, useState } from "react";
import api from "../api/api";
import { Layout, Switch, Card, Typography, Button } from "antd";
import { MoonOutlined, SunOutlined } from "@ant-design/icons";
import "antd/dist/reset.css";
import { motion } from "framer-motion";

const { Header, Content, Footer } = Layout;
const { Title, Text } = Typography;
const HomePage = () => {
  const [data, setData] = useState(null);
  const [darkMode, setDarkMode] = useState(false);
  useEffect(() => {
    api.get("/protected-data").then((res) => setData(res.data)).catch(console.error);
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
        {[1, 2, 3, 4, 5, 6].map(movie => (
          <motion.div
            key={movie}
            initial={{ opacity: 0, y: 20 }}
            animate={{ opacity: 1, y: 0 }}
            transition={{ duration: 0.5, delay: movie * 0.1 }}
          >
            <Card
              hoverable
              style={{ width: 240, borderRadius: "10px", overflow: "hidden", background: darkMode ? "#242424" : "#fff" }}
              cover={<img alt="movie poster" src={`https://via.placeholder.com/240x360?text=Movie+${movie}`} />}
            >
              <Title level={4} style={{ color: darkMode ? "#fff" : "#000" }}>Movie {movie}</Title>
              <Text style={{ color: darkMode ? "#bbb" : "#333" }}>Genre: Sci-Fi</Text>
              <div style={{ marginTop: "10px", textAlign: "center" }}>
                <Button type="primary" shape="round">Book Now</Button>
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
