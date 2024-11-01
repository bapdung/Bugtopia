package com.ssafy.bugar;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.cloud.openfeign.EnableFeignClients;

@EnableFeignClients
@SpringBootApplication
public class BugarApplication {

	public static void main(String[] args) {
		SpringApplication.run(BugarApplication.class, args);
	}

}
