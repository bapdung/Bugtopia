package com.ssafy.bugar.notification;

import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;

@RestController
public class TokenController {

    private final TokenService tokenService;

    public TokenController(TokenService tokenService) {
        this.tokenService = tokenService;
    }

    @PostMapping("/register-token")
    public String registerToken(@RequestParam String token) {
        tokenService.saveToken(token);
        return "토큰이 성공적으로 저장되었습니다!";
    }
}
