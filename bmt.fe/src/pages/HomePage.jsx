import React, { useEffect, useState } from "react";
import api from "../api/api";

const HomePage = () => {
  const [data, setData] = useState(null);

  useEffect(() => {
    api.get("/protected-data").then((res) => setData(res.data)).catch(console.error);
  }, []);

  return <div>Chào mừng! Dữ liệu bảo vệ: {JSON.stringify(data)}</div>;
};

export default HomePage;
